using System.Data.SqlClient;
using System.Data;
using DataLayer;
using Dapper;

public class ContactRepositoryEx
{
    private IDbConnection _db;

    public ContactRepositoryEx(string connString)
    {
        _db = new SqlConnection(connString);
    }

    public List<Contact> GetContactsById(params int[] ids)
    {
        return _db.Query<Contact>("SELECT * FROM Contacts WHERE ID IN @Ids", new { Ids = ids }).ToList();
    }

    public List<dynamic> GetDynamicContactsById(params int[] ids)
    {
        return _db.Query("SELECT * FROM Contacts WHERE ID IN @Ids", new { Ids = ids }).ToList();
    }

    public int BulkInsertContacts(List<Contact> contacts)
    {
        var sql =
            "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES(@FirstName, @LastName, @Email, @Company, @Title); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)";
        return _db.Execute(sql, contacts);
    }

    public List<Address> GetAddressesByState(int stateId)
    {
        return _db.Query<Address>("SELECT * FROM Addresses WHERE StateId = {=stateId}", new { stateId }).ToList();
    }

    public List<Contact> GetAllContactsWithAddresses()
    {
        var sql = "SELECT * FROM Contacts AS C INNER JOIN Addresses AS A ON A.ContactId = C.Id";

        var contactDic = new Dictionary<int, Contact>();

        var contacts = _db.Query<Contact, Address, Contact>(sql, (contact, address) =>
        {
            if (!contactDic.TryGetValue(contact.Id, out var currentContact))
            {
                currentContact = contact;   
                contactDic.Add(currentContact.Id, currentContact);
            }
            currentContact.Addresses.Add(address);             
            return currentContact;
        });

        return contacts.Distinct().ToList();
    }
}