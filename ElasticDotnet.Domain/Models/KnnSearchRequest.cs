namespace ElasticDotnet.Domain.Models;

public record KnnSearchRequest(
        int NumCandidatesDesc,
        int NumCandidatesProdLabel,
        int TopResDesc,
        int TopResProdLabel
);
