namespace Owt.Services.Contacts
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Owt.Common.Contacts;

    public interface IContactService
    {
        Task<ContactExcerptDto> CreateContactAsync(NewContactDto newContact, CancellationToken cancellationToken);

        Task<ContactDto> UpdateContactAsync(int id, ContactDto contactToUpdate, CancellationToken cancellationToken);

        Task DeleteContactAsync(int id, CancellationToken cancellationToken);

        Task<ContactDto> GetContactAsync(int id, CancellationToken cancellationToken);

        Task<List<ContactDto>> GetContactsAsync(int? limit, CancellationToken cancellationToken);

        Task<ContactDetailsDto> GetContactDetailsAsync(int id, CancellationToken cancellationToken);

        Task<List<ContactSkillDto>> GetContactSkillsAsync(int contactId, CancellationToken cancellationToken);

        Task<List<ContactSkillDto>> UpdateContactSkillAsync(int contactId, int contactSkillId, ContactSkillDto skillToUpdate, CancellationToken cancellationToken);

        Task<List<ContactSkillDto>> DeleteContactSkillAsync(int contactId, int contactSkillId, CancellationToken cancellationToken);

        Task<List<ContactSkillDto>> CreateContactSkillAsync(int contactId, NewContactSkillDto newContactSkill, CancellationToken cancellationToken);
    }
}