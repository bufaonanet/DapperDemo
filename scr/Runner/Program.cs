using DataLayer;
using Microsoft.Extensions.Configuration;
using Runner;
using System.Diagnostics;

var config = CreateBuilder().Build();

var repository = CreateRepository(config);
var repositoryEx = CreateRepositoryEx(config);

//Get_all_should_return_6_results(repository);
//var id = Insert_should_assign_identity_to_new_entity(repository);
//Find_should_retrieve_existing_entity(repository, id);
//Modify_should_update_existing_entity(repository, id);
//Delete_should_remove_entity(repository, id);

//List_support_should_produce_correct_results(repositoryEx);
//Dynamic_support_should_produce_correct_results(repositoryEx);
//Bulk_insert_should_insert_4_rows(repositoryEx);
//GetIllinoisAddresses(repositoryEx);
Get_all_should_return_6_results_with_addresses(repositoryEx);

//var mj = repository.GetFullContact(1);
//mj.Output();

static void Get_all_should_return_6_results_with_addresses(ContactRepositoryEx repository)
{
    // act
    var contacts = repository.GetAllContactsWithAddresses();

    // assert
    Console.WriteLine($"Count: {contacts.Count}");
    contacts.Output();
    Debug.Assert(contacts.Count == 6);
    Debug.Assert(contacts.First().Addresses.Count == 2);
}

static void GetIllinoisAddresses(ContactRepositoryEx repository)
{   
    // act
    var addresses = repository.GetAddressesByState(17);

    // assert
    Debug.Assert(addresses.Count == 2);
    addresses.Output();
}

static void Bulk_insert_should_insert_4_rows(ContactRepositoryEx repository)
{
    // arrange   
    var contacts = new List<Contact>
            {
                new Contact { FirstName = "Charles", LastName = "Barkley" },
                new Contact { FirstName = "Scottie", LastName = "Pippen" },
                new Contact { FirstName = "Tim", LastName = "Duncan" },
                new Contact { FirstName = "Patrick", LastName = "Ewing" }
            };

    // act
    var rowsAffected = repository.BulkInsertContacts(contacts);

    // assert
    Console.WriteLine($"Rows inserted: {rowsAffected}");
    Debug.Assert(rowsAffected == 4);
}

static void Dynamic_support_should_produce_correct_results(ContactRepositoryEx repository)
{
    // act
    var contacts = repository.GetDynamicContactsById(1, 2, 4);

    // assert
    Debug.Assert(contacts.Count == 3);
    Console.WriteLine($"First FirstName is: {contacts.First().FirstName}");
    contacts.Output();
}

static void List_support_should_produce_correct_results(ContactRepositoryEx repository)
{
    // act
    var contacts = repository.GetContactsById(1, 2, 4);

    // assert
    Debug.Assert(contacts.Count == 3);
    contacts.Output();
}

static void Delete_should_remove_entity(IContactRepository repository,int id)
{
    // act
    repository.Remove(id);
   
    var deletedEntity = repository.Find(id);

    // assert
    Debug.Assert(deletedEntity == null);
    Console.WriteLine("*** Contact Deleted ***");
}

static void Modify_should_update_existing_entity(IContactRepository repository,int id)
{    

    // act
    //var contact = repository.Find(id);
    var contact = repository.GetFullContact(id);
    contact.FirstName = "Bufão";
    contact.Addresses[0].StreetAddress = "456 Main Street";

    //repository.Update(contact);
    repository.Save(contact);
    
    //var modifiedContact = repository.Find(id);
    var modifiedContact = repository.GetFullContact(id);

    // assert
    Console.WriteLine("*** Contact Modified ***");
    modifiedContact.Output();
    Debug.Assert(modifiedContact.FirstName == "Bufão");
    Debug.Assert(modifiedContact.Addresses.First().StreetAddress == "456 Main Street");
}

static void Find_should_retrieve_existing_entity(IContactRepository repository, int id)
{
    // act
    //var contact = repository.Find(id);
    var contact = repository.GetFullContact(id);

    // assert
    Console.WriteLine("*** Get Contact ***");
    contact.Output();
    Debug.Assert(contact.FirstName == "Joe");
    Debug.Assert(contact.LastName == "Blow");
    Debug.Assert(contact.Addresses.Count == 1);
    Debug.Assert(contact.Addresses.First().StreetAddress == "123 Main Street");
}

static int Insert_should_assign_identity_to_new_entity(IContactRepository repository)
{
    // arrange    
    var contact = new Contact
    {
        FirstName = "Joe",
        LastName = "Blow",
        Email = "joe.blow@gmail.com",
        Company = "Microsoft",
        Title = "Developer"
    };
    var address = new Address
    {
        AddressType = "Home",
        StreetAddress = "123 Main Street",
        City = "Baltimore",
        StateId = 1,
        PostalCode = "22222"
    };
    contact.Addresses.Add(address);

    //act
    //repository.Add(contact);
    repository.Save(contact);

    // assert
    Debug.Assert(contact.Id != 0);
    Console.WriteLine("*** Contact Inserted ***");
    Console.WriteLine($"New ID: {contact.Id}");
    return contact.Id;
}
static void Get_all_should_return_6_results(IContactRepository repository)
{

    // act
    var contacts = repository.GetAll();

    // assert
    Console.WriteLine($"Count: {contacts.Count}");
    Debug.Assert(contacts.Count == 6);
    contacts.Output();
}


static IContactRepository CreateRepository(IConfiguration config)
{
    //return new ContactRepository(config.GetConnectionString("DefaultConnection"));
    //return new ContactRepositoryContrib(config.GetConnectionString("DefaultConnection"));
    return new ContactRepositorySP(config.GetConnectionString("docker-sqlserver"));
}

static ContactRepositoryEx CreateRepositoryEx(IConfiguration config)
{
    return new ContactRepositoryEx(config.GetConnectionString("docker-sqlserver"));
}

static IConfigurationBuilder CreateBuilder()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    return builder;

}