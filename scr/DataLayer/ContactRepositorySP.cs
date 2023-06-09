﻿using Dapper;
using DataLayer;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

public class ContactRepositorySP : IContactRepository
{
    private readonly IDbConnection _db;

    public ContactRepositorySP(string connString)
    {
        _db = new SqlConnection(connString);
    }

    public Contact Add(Contact contact)
    {
        throw new NotImplementedException();
    }

    public Contact Find(int id)
    {
        return _db.Query<Contact>("GetContact", new { Id = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
    }

    public List<Contact> GetAll()
    {
        throw new NotImplementedException();
    }

    public Contact GetFullContact(int id)
    {
        using (var multipleResults = _db.QueryMultiple("GetContact", new { Id = id }, commandType: CommandType.StoredProcedure))
        {
            var contact = multipleResults.Read<Contact>().SingleOrDefault();

            var addresses = multipleResults.Read<Address>().ToList();
            if (contact != null && addresses != null)
            {
                contact.Addresses.AddRange(addresses);
            }

            return contact;
        }
    }

    public void Remove(int id)
    {
        _db.Execute("DeleteContact", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public void Save(Contact contact)
    {
        using var txScope = new TransactionScope();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", value: contact.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
        parameters.Add("@FirstName", contact.FirstName);
        parameters.Add("@LastName", contact.LastName);
        parameters.Add("@Company", contact.Company);
        parameters.Add("@Title", contact.Title);
        parameters.Add("@Email", contact.Email);

        _db.Execute("SaveContact", parameters, commandType: CommandType.StoredProcedure);
       
        contact.Id = parameters.Get<int>("@Id");

        foreach (var addr in contact.Addresses.Where(a => !a.IsDeleted))
        {
            addr.ContactId = contact.Id;

            var addrParams = new DynamicParameters(new
            {
                ContactId = addr.ContactId,
                AddressType = addr.AddressType,
                StreetAddress = addr.StreetAddress,
                City = addr.City,
                StateId = addr.StateId,
                PostalCode = addr.PostalCode
            });
            addrParams.Add("@Id", addr.Id, DbType.Int32, ParameterDirection.InputOutput);
            _db.Execute("SaveAddress", addrParams, commandType: CommandType.StoredProcedure);
            addr.Id = addrParams.Get<int>("@Id");
        }

        foreach (var addr in contact.Addresses.Where(a => a.IsDeleted))
        {
            _db.Execute("DeleteAddress", new { Id = addr.Id }, commandType: CommandType.StoredProcedure);
        }

        txScope.Complete();
    }

    public Contact Update(Contact contact)
    {
        throw new NotImplementedException();
    }
}