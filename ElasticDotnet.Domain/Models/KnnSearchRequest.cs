namespace ElasticDotnet.Domain.Models;

public record KnnSearchRequest(
        int NumCandidatesDesc,
        int NumCandidatesProdName,
        int TopResDesc,
        int TopResProdName
);
