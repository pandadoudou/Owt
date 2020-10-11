namespace Owt.Data.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class ContactRepository : IContactRepository
    {
        private readonly OwtDbContext dbContext;

        public ContactRepository(OwtDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Contact> CreateContactAsync(Contact contact, CancellationToken cancellationToken)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }

            Contact addedContact = this.dbContext.Set<Contact>().Add(contact);

            return addedContact;
        }

        public async Task<Contact> GetContactAsync(
            Expression<Func<Contact, bool>> filter,
            CancellationToken cancellationToken)
        {
            Contact contact =
                await this.GetContactsQueryable()
                    .FirstOrDefaultAsync(filter, cancellationToken);

            return contact;
        }

        public async Task<Contact> GetContactModelWithSkillsAsync(Expression<Func<Contact, bool>> filter, CancellationToken cancellationToken)
        {
            Contact contact =
                await this.GetContactsQueryable()
                    .Include(e => e.ContactSkills)
                    .FirstOrDefaultAsync(filter, cancellationToken);

            return contact;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await this.dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Contact>> GetContactsAsync(
            int limit,
            CancellationToken cancellationToken)
        {
            IQueryable<Contact> queryable = this.GetContactsQueryable();

            List<Contact> contacts = await queryable
                                         .Take(limit)
                                         .AsNoTracking()
                                         .ToListAsync(cancellationToken);

            return contacts;
        }

        public async Task<ContactSkill> GetContactSkillAsync(int contactId, Expression<Func<ContactSkill, bool>> filter, CancellationToken cancellationToken)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            IQueryable<ContactSkill> queryable = ApplyDeletionFilter(await this.GetContactSkillsQueryableAsync(contactId, cancellationToken));

            return await queryable.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<List<ContactSkill>> GetContactSkillsAsync(int contactId, CancellationToken cancellationToken)
        {
            IQueryable<ContactSkill> queryable = ApplyDeletionFilter(await this.GetContactSkillsQueryableAsync(contactId, cancellationToken));

            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<ContactSkill> CreateContactSkillAsync(ContactSkill contactSkill, CancellationToken cancellationToken)
        {
            if (contactSkill == null)
            {
                throw new ArgumentNullException(nameof(contactSkill));
            }

            ContactSkill addedContactSkill = this.dbContext.Set<ContactSkill>().Add(contactSkill);

            return addedContactSkill;
        }

        private async Task<IQueryable<ContactSkill>> GetContactSkillsQueryableAsync(int contactId, CancellationToken cancellationToken)
        {
            IQueryable<ContactSkill> queryable = this.dbContext.Set<ContactSkill>()
                .Where(cs => cs.ContactId == contactId)
                .AsNoTracking();

            queryable = ApplyDeletionFilter(queryable);

            return queryable;
        }

        public async Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken)
        {
            await this.UpdateEntityAsync(contact, cancellationToken);

            if (contact.ContactSkills != null)
            {
                foreach (ContactSkill cs in contact.ContactSkills)
                {
                    await this.UpdateContactSkillAsync(cs, cancellationToken);
                }
            }
        }

        public async Task UpdateContactSkillAsync(ContactSkill contactSkill, CancellationToken cancellationToken)
        {
            await this.UpdateEntityAsync(contactSkill, cancellationToken);
        }

        private async Task UpdateEntityAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
            where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.dbContext
                .Set<TEntity>()
                .Attach(entity);

            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<Contact> GetContactsQueryable(params Expression<Func<Contact, object>>[] includes)
        {
            IQueryable<Contact> queryable = this.dbContext
                .Set<Contact>()
                .AsQueryable();

            foreach (Expression<Func<Contact, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            queryable = ApplyDeletionFilter(queryable);

            return queryable;
        }

        private static IQueryable<TDeletable> ApplyDeletionFilter<TDeletable>(IQueryable<TDeletable> queryable)
            where TDeletable : class, IDeletable =>
            queryable.Where(c => !c.Deleted);
    }
}