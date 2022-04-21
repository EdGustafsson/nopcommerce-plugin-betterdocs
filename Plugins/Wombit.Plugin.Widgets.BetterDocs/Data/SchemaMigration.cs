using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Wombit.Plugin.Widgets.BetterDocs.Domain;

namespace Wombit.Plugin.Widgets.BetterDocs.Data
{
    [NopMigration("2022/03/23 11:35:55:1687541", "Widgets.Document base schema")]
    public class SchemaMigration : AutoReversingMigration
    {

        private readonly IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public override void Up()
        {
            _migrationManager.BuildTable<Document>(Create);
            _migrationManager.BuildTable<DocumentMapping>(Create);
        }
    }
}