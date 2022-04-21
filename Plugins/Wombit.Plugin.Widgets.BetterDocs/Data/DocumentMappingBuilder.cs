using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Data.Extensions;



namespace Nop.Plugin.Shipping.FixedByWeightByTotal.Data
{
    public partial class DocumentMappingBuilder : NopEntityBuilder<DocumentMapping>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                 .WithColumn(nameof(DocumentMapping.DocumentId)).AsInt32().ForeignKey<Document>();
        }
    }
}