using DatabaseDefinition.EntityModel.Repositories;
using DatabaseDefintion.EntityModel;

namespace DatabaseDefintion.Test
{
    public class DatabaseDefinitionTestBase
    {
        protected ItemRepository ItemRepository { get; }
        protected ItemPictureRepository ItemPictureRepository { get; }
        protected CancellationToken CancellationToken { get; } = CancellationToken.None;

        public DatabaseDefinitionTestBase()
        {
            string[] args = new string[5];
            var factory = new AppProjectDbContextFactory();
            var dbContext = factory.CreateDbContext(args);
            ItemRepository = new(dbContext);
            ItemPictureRepository = new(dbContext);
        }
    }
}
