using Dapper.Contrib.Extensions;
using DataLayer;
using System.Data;
using System.Data.SqlClient;

public class ContactRepositoryContrib : IContactRepository
{
    private readonly IDbConnection _db;

    public ContactRepositoryContrib(string connString)
    {
        _db = new SqlConnection(connString);
    }

    public Contact Add(Contact contact)
    {
        var id = _db.Insert(contact);
        contact.Id = (int)id;
        return contact;
    }

    public Contact Find(int id)
    {
        return _db.Get<Contact>(id);
    }

    public List<Contact> GetAll()
    {
        return _db.GetAll<Contact>().ToList();
    }

    public Contact GetFullContact(int id)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        _db.Delete(new Contact { Id = id });
    }

    public void Save(Contact contact)
    {
        throw new NotImplementedException();
    }

    public Contact Update(Contact contact)
    {
        _db.Update(contact);
        return contact;
    }
}