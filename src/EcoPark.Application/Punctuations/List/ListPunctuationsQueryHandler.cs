using EcoPark.Application.Punctuations.Models;

namespace EcoPark.Application.Punctuations.List;

public class ListPunctuationsQueryHandler(IRepository<PunctuationModel> repository) : IHandler<ListPunctuationsQuery, IEnumerable<PunctuationViewModel>?>
{
    public async Task<IEnumerable<PunctuationViewModel>?> HandleAsync(ListPunctuationsQuery command,
        CancellationToken cancellationToken)
    {
        var punctuations = await repository.ListAsync(command, cancellationToken);

        if (punctuations == null || !punctuations.Any())
            return Enumerable.Empty<PunctuationViewModel>();

        List<PunctuationViewModel> result = new(punctuations.Count());

        foreach (var punctuation in punctuations)
        {
            PunctuationViewModel model = new(punctuation.ClientId, punctuation.Location.Name, punctuation.Punctuation);

            result.Add(model);
        }

        return result;
    }
}