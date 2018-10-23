using System.Threading.Tasks;
using System.Collections.Generic;

using FSBO.DAL;

namespace FSBO.WebServices.Services
{
    public class SourceService : ISourceService
    {
        private IFsboContext DbContext;

        public SourceService(IFsboContext dbContext)
        {
            DbContext = dbContext ?? new FsboContext();
        }

        public IEnumerable<Source> GetSources()
        {
            var dbSources = DbContext.Sources
                .Include("SourceAreaEntries");
            return dbSources;
        }

        public async Task<int> AddSource(Source source)
        {
            DbContext.Sources.Add(source);
            await DbContext.SaveChangesAsync();

            return source.SourceId;
        }

    }
}