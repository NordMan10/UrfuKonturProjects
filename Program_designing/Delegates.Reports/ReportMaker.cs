using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delegates.Reports
{
	public class ReportMaker<TMarkupTransformer>
		where TMarkupTransformer : IMarkupTransformer, new()
    {
		public ReportMaker(IEnumerable<Measurement> measurements)
		{
			Statistics = new Statistics(measurements);
			MarkupTransformer = new TMarkupTransformer();
		}

		public Statistics Statistics { get; }

		public TMarkupTransformer MarkupTransformer { get; }

		public string MakeReport(Tuple<string, string, string> resultData)
		{
			var result = new StringBuilder();

			result.Append(MarkupTransformer.MakeCaption(resultData.Item1));
			result.Append(MarkupTransformer.BeginList());
			result.Append(MarkupTransformer.MakeItem("Temperature", resultData.Item2));
			result.Append(MarkupTransformer.MakeItem("Humidity", resultData.Item3));
			result.Append(MarkupTransformer.EndList());

			return result.ToString();
		}
	}

	public interface IStatistics
    {
		string GetMeanAndStd(IEnumerable<double> data);

		string GetMedian(IEnumerable<double> data);

		Tuple<string, string, string> GetMeanAndStdResult();
		Tuple<string, string, string> GetMedianResult();
	}

	public class Statistics : IStatistics
	{
		public Statistics(IEnumerable<Measurement> measurements)
        {
			this.measurements = measurements;
        }

		private readonly IEnumerable<Measurement> measurements;

		public string GetMeanAndStd(IEnumerable<double> data_)
		{
			var data = data_.ToList();
			var mean = data.Average();
			var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

			var result = new MeanAndStd() { Mean = mean, Std = std };

			return result.ToString();
		}

		public Tuple<string, string, string> GetMeanAndStdResult()
		{
			var tempreratureData = GetMeanAndStd(measurements.Select(z => z.Temperature)).ToString();
			var humidityData = GetMeanAndStd(measurements.Select(z => z.Humidity)).ToString();

			return Tuple.Create("Mean and Std", tempreratureData, humidityData);
		}

		public string GetMedian(IEnumerable<double> data)
		{
			var list = data.OrderBy(z => z).ToList();
			if (list.Count % 2 == 0)
				return ((list[list.Count / 2] + list[list.Count / 2 - 1]) / 2).ToString();

			return list[list.Count / 2].ToString();
		}

		public Tuple<string, string, string> GetMedianResult()
		{
			var tempreratureData = GetMedian(measurements.Select(z => z.Temperature)).ToString();
			var humidityData = GetMedian(measurements.Select(z => z.Humidity)).ToString();

			return Tuple.Create("Median", tempreratureData, humidityData);
		}
	}

	public interface IMarkupTransformer
    {
		string MakeCaption(string caption);
		string BeginList();
		string EndList();
		string MakeItem(string valueType, string entry);
	}

	public class HtmlTransformer : IMarkupTransformer
    {
        public string BeginList() { return "<ul>"; }

        public string EndList() { return "</ul>"; }

        public string MakeCaption(string caption)
        {
			return $"<h1>{caption}</h1>";
		}

        public string MakeItem(string valueType, string entry)
        {
			return $"<li><b>{valueType}</b>: {entry}";
		}
    }

    public class MarkDownTransformer : IMarkupTransformer
    {
        public string BeginList() { return ""; }

        public string EndList() { return ""; }

		public string MakeCaption(string caption) { return $"## {caption}\n\n"; }

        public string MakeItem(string valueType, string entry) { return $" * **{valueType}**: {entry}\n\n"; }
    }

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> measurements)
		{
			var htmlReportMaker = new ReportMaker<HtmlTransformer>(measurements);
			return htmlReportMaker.MakeReport(htmlReportMaker.Statistics.GetMeanAndStdResult());
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> measurements)
		{
			var markDownReportMaker = new ReportMaker<MarkDownTransformer>(measurements);
			return markDownReportMaker.MakeReport(markDownReportMaker.Statistics.GetMedianResult());
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			var markDownReportMaker = new ReportMaker<MarkDownTransformer>(measurements);
			return markDownReportMaker.MakeReport(markDownReportMaker.Statistics.GetMeanAndStdResult());
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			var htmlReportMaker = new ReportMaker<HtmlTransformer>(measurements);
			return htmlReportMaker.MakeReport(htmlReportMaker.Statistics.GetMedianResult());
		}
	}
}
