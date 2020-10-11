namespace Owt.Services.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using Owt.Common.Contacts;
    using Owt.Data.Contacts;
    using Owt.Services.Security;

    public class ContactService : IContactService
    {
        private readonly IMapper mapper;
        private readonly IContactRepository contactRepository;
        private readonly IContactValidator contactValidator;
        private readonly ISecurityService securityService;

        public ContactService(
            IMapper mapper,
            IContactRepository contactRepository,
            IContactValidator contactValidator,
            ISecurityService securityService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            this.contactValidator = contactValidator ?? throw new ArgumentNullException(nameof(contactValidator));
            this.securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }

        public async Task<ContactExcerptDto> CreateContactAsync(NewContactDto newContact, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.Contact, SecurityMethods.Create);

            if (newContact == null)
            {
                throw new ArgumentNullException(nameof(newContact));
            }

            this.contactValidator.ValidateContact(newContact);
            Contact newContactModel = this.mapper.Map<Contact>(newContact);
            Contact createdContact = await this.contactRepository.CreateContactAsync(newContactModel, cancellationToken);
            createdContact.CreatedBy = this.securityService.GetCurrentIdentityName();

            await this.contactRepository.SaveChangesAsync(cancellationToken);

            ContactExcerptDto contactExcerpt = this.mapper.Map<ContactExcerptDto>(createdContact);

            return contactExcerpt;
        }

        public async Task<ContactDto> UpdateContactAsync(int id, ContactDto contactToUpdate, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.Contact, SecurityMethods.Update);

            if (contactToUpdate == null)
            {
                throw new ArgumentNullException(nameof(contactToUpdate));
            }

            if (contactToUpdate.Id != id)
            {
                throw new ArgumentException("Id mismatch.");
            }

            this.contactValidator.ValidateContact(contactToUpdate);
            Contact contact = await this.GetContactModelAsync(id, cancellationToken);

            this.securityService.EnsureOwnership(contact);

            contact = this.mapper.Map(contactToUpdate, contact);
            await this.contactRepository.UpdateContactAsync(contact, cancellationToken);

            await this.contactRepository.SaveChangesAsync(cancellationToken);

            return await this.GetContactAsync(id, cancellationToken);
        }

        public async Task DeleteContactAsync(int id, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.Contact, SecurityMethods.Delete);

            Contact contact = await this.GetContactModelWithSkillsAsync(id, cancellationToken);

            this.securityService.EnsureOwnership(contact);

            contact.Deleted = true;

            foreach (ContactSkill cs in contact.ContactSkills)
            {
                cs.Deleted = true;
            }

            await this.contactRepository.UpdateContactAsync(contact, cancellationToken);
            await this.contactRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<ContactDto> GetContactAsync(int id, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.Contact, SecurityMethods.View);

            Contact contact = await this.GetContactModelAsync(id, cancellationToken);
            ContactDto contactDto = this.mapper.Map<ContactDto>(contact);

            return contactDto;
        }

        public async Task<ContactDetailsDto> GetContactDetailsAsync(int id, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.Contact, SecurityMethods.View);

            Contact contact = await this.GetContactModelWithSkillsAsync(id, cancellationToken);
            ContactDetailsDto contactDto = this.mapper.Map<ContactDetailsDto>(contact);

            return contactDto;
        }

        public async Task<List<ContactDto>> GetContactsAsync(int? limit, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.Contact, SecurityMethods.View);

            this.securityService.Ensure(SecurityEntityTypes.Contact, SecurityMethods.View);

            List<Contact> contacts = await this.contactRepository.GetContactsAsync(this.GetLimit(limit), cancellationToken);
            List<ContactDto> contactsDto = contacts.Select(this.mapper.Map<ContactDto>).ToList();

            return contactsDto;
        }

        public async Task<List<ContactSkillDto>> GetContactSkillsAsync(int contactId, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.ContactSkills, SecurityMethods.View);

            List<ContactSkill> contactSkills = await this.contactRepository.GetContactSkillsAsync(contactId, cancellationToken);
            List<ContactSkillDto> contactSkillsDto = contactSkills.Select(this.mapper.Map<ContactSkillDto>).ToList();

            return contactSkillsDto;
        }

        public async Task<List<ContactSkillDto>> UpdateContactSkillAsync(int contactId, int contactSkillId, ContactSkillDto skillToUpdate, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.ContactSkills, SecurityMethods.Update);

            if (skillToUpdate == null)
            {
                throw new ArgumentNullException(nameof(skillToUpdate));
            }

            if (skillToUpdate.ContactId != contactId)
            {
                throw new ArgumentException("ContactId mismatch.");
            }

            if (skillToUpdate.Id != contactSkillId)
            {
                throw new ArgumentException("ContactSkillId mismatch.");
            }

            Contact contact = await this.GetContactModelAsync(contactId, cancellationToken);
            this.securityService.EnsureOwnership(contact);

            await this.CheckIfContactSkillExistsAsync(contactId, contactSkillId, cancellationToken);

            ContactSkill contactSkill = await this.GetContactSkillModelAsync(contactId, contactSkillId, true, cancellationToken);
            this.securityService.EnsureOwnership(contactSkill);

            contactSkill = this.mapper.Map(skillToUpdate, contactSkill);
            await this.contactRepository.UpdateContactSkillAsync(contactSkill, cancellationToken);
            await this.contactRepository.SaveChangesAsync(cancellationToken);

            return await this.GetContactSkillsAsync(contactId, cancellationToken);
        }

        public async Task<List<ContactSkillDto>> DeleteContactSkillAsync(int contactId, int contactSkillId, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.ContactSkills, SecurityMethods.Delete);

            Contact contact = await this.GetContactModelAsync(contactId, cancellationToken);
            this.securityService.EnsureOwnership(contact);

            ContactSkill contactSkill = await this.GetContactSkillModelAsync(contactId, contactSkillId, true, cancellationToken);
            this.securityService.EnsureOwnership(contactSkill);

            contactSkill.Deleted = true;
            await this.contactRepository.UpdateContactSkillAsync(contactSkill, cancellationToken);
            await this.contactRepository.SaveChangesAsync(cancellationToken);

            return await this.GetContactSkillsAsync(contactId, cancellationToken);
        }

        public async Task<List<ContactSkillDto>> CreateContactSkillAsync(int contactId, NewContactSkillDto newContactSkill, CancellationToken cancellationToken)
        {
            this.securityService.Ensure(SecurityEntityTypes.ContactSkills, SecurityMethods.Create);

            if (newContactSkill == null)
            {
                throw new ArgumentNullException(nameof(newContactSkill));
            }

            if (newContactSkill.ContactId != contactId)
            {
                throw new ArgumentException("ContactId mismatch.");
            }

            Contact contact = await this.GetContactModelAsync(contactId, cancellationToken);
            this.securityService.EnsureOwnership(contact);

            await this.CheckIfContactSkillExistsAsync(contactId, newContactSkill.Skill.Id, cancellationToken);

            ContactSkill newContactSkillModel = this.mapper.Map<ContactSkill>(newContactSkill);
            newContactSkillModel.CreatedBy = this.securityService.GetCurrentIdentityName();

            await this.contactRepository.CreateContactSkillAsync(newContactSkillModel, cancellationToken);
            await this.contactRepository.SaveChangesAsync(cancellationToken);

            return await this.GetContactSkillsAsync(contactId, cancellationToken);
        }

        private async Task CheckIfContactSkillExistsAsync(int contactId, int skillId, CancellationToken cancellationToken)
        {
            ContactSkill contactSkill = await this.GetContactSkillModelBySkillAsync(contactId, skillId, false, cancellationToken);
            if (contactSkill != null)
            {
                throw new ArgumentException("Skill already exists for this contact.");
            }
        }

        private async Task<ContactSkill> GetContactSkillModelAsync(int contactId, int contactSkillId, bool throwExceptionIfNull, CancellationToken cancellationToken)
        {
            ContactSkill contactSkill = await this.contactRepository.GetContactSkillAsync(contactId, cs => cs.Id == contactSkillId, cancellationToken);

            if (throwExceptionIfNull && contactSkill == null)
            {
                throw new ArgumentException($"There is no {nameof(ContactSkill)} with {nameof(ContactSkill.ContactId)} {contactId} and {nameof(ContactSkill.Id)} {contactSkillId}.");
            }

            return contactSkill;
        }

        private async Task<ContactSkill> GetContactSkillModelBySkillAsync(int contactId, int skillId, bool throwExceptionIfNull, CancellationToken cancellationToken)
        {
            ContactSkill contactSkill = await this.contactRepository.GetContactSkillAsync(contactId, cs => cs.SkillId == skillId, cancellationToken);

            if (throwExceptionIfNull && contactSkill == null)
            {
                throw new ArgumentException($"There is no {nameof(ContactSkill)} with {nameof(ContactSkill.ContactId)} {contactId} and {nameof(ContactSkill.SkillId)} {skillId}.");
            }

            return contactSkill;
        }

        private async Task<Contact> GetContactModelAsync(int id, CancellationToken cancellationToken)
        {
            Contact contact = await this.contactRepository.GetContactAsync(e => e.Id == id, cancellationToken);

            if (contact == null)
            {
                throw new ArgumentException($"Unable to find contact with id '{id}'.");
            }

            return contact;
        }

        private async Task<Contact> GetContactModelWithSkillsAsync(int id, CancellationToken cancellationToken)
        {
            Contact contact = await this.contactRepository.GetContactModelWithSkillsAsync(e => e.Id == id, cancellationToken);

            if (contact == null)
            {
                throw new ArgumentException($"Unable to find contact with id '{id}'.");
            }

            contact.ContactSkills = contact.ContactSkills.Where(cs => !cs.Deleted).ToList();

            return contact;
        }

        private int GetLimit(int? limit)
        {
            const int MaxLimit = 50;

            return Math.Min(limit ?? MaxLimit, MaxLimit);
        }
    }
}