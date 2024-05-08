using EcoPark.Application.Punctuations.Models;

namespace EcoPark.Application.Punctuations.Get;

public class GetPunctuationQueryHandler(IRepository<PunctuationModel> repository) : IHandler<GetPunctuationQuery, PunctuationViewModel?>
{
    public async Task<PunctuationViewModel?> HandleAsync(GetPunctuationQuery command, CancellationToken cancellationToken)
    {
        var punctuation = await repository.GetByIdAsync(command, cancellationToken);
        PunctuationViewModel? result = null;

        if (punctuation != null)
            result = new(punctuation.ClientId, punctuation.Location.Name, punctuation.Punctuation);

        return result;
    }
}