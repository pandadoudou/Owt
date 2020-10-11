namespace Owt.Data.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IContactRepository
    {
        Task<Contact> CreateContactAsync(Contact contact, CancellationToken cancellationToken);

        Task<Contact> GetContactAsync(Expression<Func<Contact, bool>> filter, CancellationToken cancellationToken);

        Task<Contact> GetContactModelWithSkillsAsync(Expression<Func<Contact, bool>> filter, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task<List<Contact>> GetContactsAsync(int limit, CancellationToken cancellationToken);

        Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken);

        Task UpdateContactSkillAsync(ContactSkill contactSkill, CancellationToken cancellationToken);

        Task<ContactSkill> GetContactSkillAsync(int contactId, Expression<Func<ContactSkill, bool>> filter, CancellationToken cancellationToken);

        Task<List<ContactSkill>> GetContactSkillsAsync(int contactId, CancellationToken cancellationToken);

        Task<ContactSkill> CreateContactSkillAsync(ContactSkill contactSkill, CancellationToken cancellationToken);
    }
}