using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Wombit.Plugin.Widgets.BetterDocs.Models;

namespace Wombit.Plugin.Widgets.BetterDocs.Validators
{
    public class DocumentValidator : BaseNopValidator<DocumentModel>
    {
        public DocumentValidator(ILocalizationService localizationService)
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessageAwait(
            //    localizationService.GetResourceAsync("Wombit.Document.Required"), nameof(DocumentModel.Title));
            //RuleFor(x => x.DownloadId).NotEmpty().WithMessageAwait(
            //    localizationService.GetResourceAsync("Wombit.Document.Required"), localizationService.GetResourceAsync("Wombit.Document.Fields.DownloadId"));
        }
    }
}
