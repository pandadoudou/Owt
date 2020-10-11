namespace Owt.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Owt.Common.Lookups;
    using Owt.Services.Lookups;

    /// <summary>
    /// Allow to handle lookups
    /// </summary>
    [RoutePrefix("api")]
    public class LookupController : ApiController
    {
        private readonly ILookupService lookupService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lookupService"></param>
        public LookupController(ILookupService lookupService)
        {
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }

        /// <summary>
        /// Returns skills
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("skills")]
        public async Task<IHttpActionResult> GetSkillsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<LookupDto> skills = await this.lookupService.GetSkillsAsync(cancellationToken);

            return this.Ok(skills);
        }

        /// <summary>
        /// Returns a skill
        /// </summary>
        /// <param name="id">Id of the skill</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("skills/{id:int}")]
        public async Task<IHttpActionResult> GetSkillAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            LookupDto skill = await this.lookupService.GetSkillAsync(id, cancellationToken);

            return this.Ok(skill);
        }

        /// <summary>
        /// Returns expertise levels
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("expertiseLevels")]
        public async Task<IHttpActionResult> GetExpertiseLevelsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<LookupDto> expertiseLevels = await this.lookupService.GetExpertiseLevelsAsync(cancellationToken);

            return this.Ok(expertiseLevels);
        }

        /// <summary>
        /// Returns an expertise level
        /// </summary>
        /// <param name="id">Id of the expertise level</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("expertiseLevels/{id:int}")]
        public async Task<IHttpActionResult> GetExpertiseLevelAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            LookupDto expertiseLevel = await this.lookupService.GetExpertiseLevelAsync(id, cancellationToken);

            return this.Ok(expertiseLevel);
        }
    }
}