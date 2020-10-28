using System;
using System.Collections.Generic;

using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        private ComparisonResult GetDistanceBetween(DocumentTokens firstDocument, DocumentTokens secondDocument)
        {
            var optimal = new double[firstDocument.Count + 1, secondDocument.Count + 1];
            for (var i = 0; i < secondDocument.Count + 1; ++i) optimal[0, i] = i;
            for (var i = 0; i < firstDocument.Count + 1; ++i) optimal[i, 0] = i;
            for (var i = 1; i < firstDocument.Count + 1; ++i)
                for (var j = 1; j < secondDocument.Count + 1; ++j)
                {
                    var distance = TokenDistanceCalculator.GetTokenDistance(firstDocument[i - 1], secondDocument[j - 1]);
                    if (distance != 0)
                        optimal[i, j] = Math.Min(Math.Min(optimal[i - 1, j] + 1, optimal[i, j - 1] + 1), optimal[i - 1, j - 1] + distance);
                    else if (distance == 0) optimal[i, j] = optimal[i - 1, j - 1];
                }
            return new ComparisonResult(firstDocument, secondDocument, optimal[firstDocument.Count, secondDocument.Count]);
        }

        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var compResult = new List<ComparisonResult>();
            for (var i = 0; i < documents.Count; i++)
                for (var j = i + 1; j < documents.Count; j++)
                    compResult.Add(GetDistanceBetween(documents[i], documents[j]));
            return compResult;
        }
    }
}