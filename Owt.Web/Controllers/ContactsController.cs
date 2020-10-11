namespace Owt.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Owt.Common.Contacts;
    using Owt.Services.Contacts;

    /// <summary>
    /// Allow to handle contact and its skills
    /// </summary>
    [RoutePrefix("api/contacts")]
    [Authorize]
    public class ContactsController : ApiController
    {
        private readonly IContactService contactService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contactService"></param>
        public ContactsController(IContactService contactService)
        {
            this.contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        }

        /// <summary>
        /// Returns a list of contact
        /// </summary>
        /// <param name="limit">Limits the output</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of contact</returns>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetContactsAsync(int? limit = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ContactDto> contacts = await this.contactService.GetContactsAsync(limit, cancellationToken);

            return this.Ok(contacts);
        }

        /// <summary>
        /// Get a contact by its id
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A contact</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetContactAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            ContactDto contact = await this.contactService.GetContactAsync(id, cancellationToken);

            return this.Ok(contact);
        }

        /// <summary>
        /// Returns contact with its skills
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A contact with its skills</returns>
        [HttpGet]
        [Route("{id:int}/details")]
        public async Task<IHttpActionResult> GetContactDetailsAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            ContactDetailsDto contact = await this.contactService.GetContactDetailsAsync(id, cancellationToken);

            return this.Ok(contact);
        }

        /// <summary>
        /// Returns skills of a contact
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of contact's skills</returns>
        [HttpGet]
        [Route("{id:int}/skills")]
        public async Task<IHttpActionResult> GetContactSkillsAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ContactSkillDto> contactSkills = await this.contactService.GetContactSkillsAsync(id, cancellationToken);

            return this.Ok(contactSkills);
        }

        /// <summary>
        /// Create a contact
        /// </summary>
        /// <param name="newContact">Contact to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The created contact excerpt</returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateContactAsync(NewContactDto newContact, CancellationToken cancellationToken = default(CancellationToken))
        {
            ContactExcerptDto contactExcerpt = await this.contactService.CreateContactAsync(newContact, cancellationToken);

            return this.Ok(contactExcerpt);
        }

        /// <summary>
        /// Add a skill to a contact
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="newContactSkill">Skill to add</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of contact's skills</returns>
        [HttpPost]
        [Route("{id:int}/skills")]
        public async Task<IHttpActionResult> CreateContactSkillAsync(int id, NewContactSkillDto newContactSkill, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ContactSkillDto> contactSkills = await this.contactService.CreateContactSkillAsync(id, newContactSkill, cancellationToken);

            return this.Ok(contactSkills);
        }

        /// <summary>
        /// Update a contact
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="contactToUpdate">Contact values to udpate</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The updated contact</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateContactAsync(int id, ContactDto contactToUpdate, CancellationToken cancellationToken = default(CancellationToken))
        {
            ContactDto contact = await this.contactService.UpdateContactAsync(id, contactToUpdate, cancellationToken);

            return this.Ok(contact);
        }

        /// <summary>
        /// Update a contact skill
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="contactSkillId">Id of the skill</param>
        /// <param name="skillToUpdate">Values of the skill to update</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of contact's skills</returns>
        [HttpPut]
        [Route("{id:int}/skills/{contactSkillId:int}")]
        public async Task<IHttpActionResult> UpdateContactSkillAsync(int id, int contactSkillId, ContactSkillDto skillToUpdate, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ContactSkillDto> contactSkills = await this.contactService.UpdateContactSkillAsync(id, contactSkillId, skillToUpdate, cancellationToken);

            return this.Ok(contactSkills);
        }

        /// <summary>
        /// Soft delete a contact
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}/delete")]
        public async Task<IHttpActionResult> DeleteContactAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.contactService.DeleteContactAsync(id, cancellationToken);

            return this.Ok();
        }

        /// <summary>
        /// Soft delete a contact skill
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="contactSkillId">Id of the contact skill</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}/skills/{contactSkillId:int}/delete")]
        public async Task<IHttpActionResult> DeleteContactSkillAsync(int id, int contactSkillId, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ContactSkillDto> contactSkills = await this.contactService.DeleteContactSkillAsync(id, contactSkillId, cancellationToken);

            return this.Ok(contactSkills);
        }
    }
}