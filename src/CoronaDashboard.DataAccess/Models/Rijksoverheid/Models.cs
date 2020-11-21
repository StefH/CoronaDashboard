using System;
using System.Text.Json.Serialization;

namespace CoronaDashboard.Models.Rijksoverheid
{
    public class Covid19RootObject
    {
        [JsonPropertyName("last_generated")]
        public string LastGenerated { get; set; }

        [JsonPropertyName("proto_name")]
        public string ProtoName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("verdenkingen_huisartsen")]
        public VerdenkingenHuisartsen VerdenkingenHuisartsen { get; set; }

        [JsonPropertyName("intake_hospital_ma")]
        public IntakeHospitalMa IntakeHospitalMa { get; set; }

        [JsonPropertyName("infectious_people_count")]
        public InfectiousPeople InfectiousPeopleCount { get; set; }

        [JsonPropertyName("infectious_people_count_normalized")]
        public InfectiousPeopleCountNormalized InfectiousPeopleCountNormalized { get; set; }

        [JsonPropertyName("intake_intensivecare_ma")]
        public IntakeIntensivecareMa IntakeIntensivecareMa { get; set; }

        [JsonPropertyName("infected_people_total")]
        public InfectedPeopleTotal InfectedPeopleTotal { get; set; }

        [JsonPropertyName("infected_people_delta_normalized")]
        public InfectedPeopleDeltaNormalized InfectedPeopleDeltaNormalized { get; set; }

        [JsonPropertyName("intake_share_age_groups")]
        public IntakeShareAgeGroups IntakeShareAgeGroups { get; set; }

        [JsonPropertyName("reproduction_index")]
        public ReproductionIndex ReproductionIndex { get; set; }

        [JsonPropertyName("reproduction_index_last_known_average")]
        public ReproductionIndex ReproductionIndexLastKnownAverage { get; set; }

        [JsonPropertyName("sewer")]
        public Sewer Sewer { get; set; }

        [JsonPropertyName("sewer_per_installation")]
        public SewerPerInstallation SewerPerInstallation { get; set; }

        [JsonPropertyName("ggd")]
        public Ggd Ggd { get; set; }

        [JsonPropertyName("intensive_care_beds_occupied")]
        public IntensiveCareBedsOccupied IntensiveCareBedsOccupied { get; set; }

        [JsonPropertyName("hospital_beds_occupied")]
        public HospitalBedsOccupied HospitalBedsOccupied { get; set; }

        [JsonPropertyName("nursing_home")]
        public NursingHome NursingHome { get; set; }

        [JsonPropertyName("infectious_people_last_known_average")]
        public InfectiousPeople InfectiousPeopleLastKnownAverage { get; set; }

        [JsonPropertyName("date_processed")]
        public DateTimeOffset DateProcessed { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("last_generated_DateTime")]
        public DateTimeOffset LastGeneratedDateTime { get; set; }

        //[JsonPropertyName("_rid")]
        //public string Rid { get; set; }

        //[JsonPropertyName("_self")]
        //public string Self { get; set; }

        //[JsonPropertyName("_etag")]
        //public string Etag { get; set; }

        //[JsonPropertyName("_attachments")]
        //public string Attachments { get; set; }

        //[JsonPropertyName("_ts")]
        //public long Ts { get; set; }
    }

    public class Ggd
    {
        [JsonPropertyName("values")]
        public GgdLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public GgdLastValue LastValue { get; set; }
    }

    public class GgdLastValue
    {
        [JsonPropertyName("week_unix")]
        public long WeekUnix { get; set; }

        [JsonPropertyName("week_start_unix")]
        public long WeekStartUnix { get; set; }

        [JsonPropertyName("week_end_unix")]
        public long WeekEndUnix { get; set; }

        [JsonPropertyName("infected")]
        public long Infected { get; set; }

        [JsonPropertyName("infected_percentage")]
        public double InfectedPercentage { get; set; }

        [JsonPropertyName("tested_total")]
        public long TestedTotal { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class HospitalBedsOccupied
    {
        [JsonPropertyName("values")]
        public HospitalBedsOccupiedLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public HospitalBedsOccupiedLastValue LastValue { get; set; }
    }

    public class HospitalBedsOccupiedLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("covid_occupied")]
        public long CovidOccupied { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class InfectedPeopleDeltaNormalized
    {
        [JsonPropertyName("values")]
        public InfectedPeopleDeltaNormalizedLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public InfectedPeopleDeltaNormalizedLastValue LastValue { get; set; }
    }

    public class InfectedPeopleDeltaNormalizedLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("infected_daily_increase")]
        public double InfectedDailyIncrease { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class InfectedPeopleTotal
    {
        [JsonPropertyName("values")]
        public InfectedPeopleTotalValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public InfectedPeopleTotalValue LastValue { get; set; }
    }

    public class InfectedPeopleTotalValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("infected_daily_total")]
        public long InfectedDailyTotal { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class InfectiousPeople
    {
        [JsonPropertyName("values")]
        public InfectiousPeopleCountLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public InfectiousPeopleCountLastValue LastValue { get; set; }
    }

    public class InfectiousPeopleCountLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("infectious_low")]
        public long InfectiousLow { get; set; }

        [JsonPropertyName("infectious_avg")]
        public long? InfectiousAvg { get; set; }

        [JsonPropertyName("infectious_high")]
        public long InfectiousHigh { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class InfectiousPeopleCountNormalized
    {
        [JsonPropertyName("values")]
        public InfectiousPeopleCountNormalizedLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public InfectiousPeopleCountNormalizedLastValue LastValue { get; set; }
    }

    public class InfectiousPeopleCountNormalizedLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("infectious_low_normalized")]
        public double InfectiousLowNormalized { get; set; }

        [JsonPropertyName("infectious_avg_normalized")]
        public double? InfectiousAvgNormalized { get; set; }

        [JsonPropertyName("infectious_high_normalized")]
        public double InfectiousHighNormalized { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class IntakeHospitalMa
    {
        [JsonPropertyName("values")]
        public IntakeHospitalMaLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public IntakeHospitalMaLastValue LastValue { get; set; }
    }

    public class IntakeHospitalMaLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("moving_average_hospital")]
        public double MovingAverageHospital { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class IntakeIntensivecareMa
    {
        [JsonPropertyName("values")]
        public IntakeIntensivecareMaLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public IntakeIntensivecareMaLastValue LastValue { get; set; }
    }

    public class IntakeIntensivecareMaLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("moving_average_ic")]
        public double MovingAverageIc { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class IntakeShareAgeGroups
    {
        [JsonPropertyName("values")]
        public IntakeShareAgeGroupsLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public IntakeShareAgeGroupsLastValue LastValue { get; set; }
    }

    public class IntakeShareAgeGroupsLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("agegroup")]
        public string Agegroup { get; set; }

        [JsonPropertyName("infected_per_agegroup_increase")]
        public long InfectedPerAgegroupIncrease { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class IntensiveCareBedsOccupied
    {
        [JsonPropertyName("values")]
        public IntensiveCareBedsOccupiedLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public IntensiveCareBedsOccupiedLastValue LastValue { get; set; }
    }

    public class IntensiveCareBedsOccupiedLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("covid_occupied")]
        public long CovidOccupied { get; set; }

        [JsonPropertyName("non_covid_occupied")]
        public long NonCovidOccupied { get; set; }

        [JsonPropertyName("covid_percentage_of_all_occupied")]
        public double CovidPercentageOfAllOccupied { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class NursingHome
    {
        [JsonPropertyName("values")]
        public NursingHomeLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public NursingHomeLastValue LastValue { get; set; }
    }

    public class NursingHomeLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("newly_infected_people")]
        public long NewlyInfectedPeople { get; set; }

        [JsonPropertyName("infected_locations_total")]
        public long InfectedLocationsTotal { get; set; }

        [JsonPropertyName("deceased_daily")]
        public long DeceasedDaily { get; set; }

        [JsonPropertyName("newly_infected_locations")]
        public long NewlyInfectedLocations { get; set; }

        [JsonPropertyName("infected_locations_percentage")]
        public double InfectedLocationsPercentage { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class ReproductionIndex
    {
        [JsonPropertyName("values")]
        public ReproductionIndexLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public ReproductionIndexLastValue LastValue { get; set; }
    }

    public class ReproductionIndexLastValue
    {
        [JsonPropertyName("date_of_report_unix")]
        public long DateOfReportUnix { get; set; }

        [JsonPropertyName("reproduction_index_low")]
        public double? ReproductionIndexLow { get; set; }

        [JsonPropertyName("reproduction_index_avg")]
        public double? ReproductionIndexAvg { get; set; }

        [JsonPropertyName("reproduction_index_high")]
        public double? ReproductionIndexHigh { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class Sewer
    {
        [JsonPropertyName("values")]
        public SewerLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public SewerLastValue LastValue { get; set; }
    }

    public class SewerLastValue
    {
        [JsonPropertyName("week_unix")]
        public long WeekUnix { get; set; }

        [JsonPropertyName("week_start_unix")]
        public long WeekStartUnix { get; set; }

        [JsonPropertyName("week_end_unix")]
        public long WeekEndUnix { get; set; }

        [JsonPropertyName("average")]
        public double Average { get; set; }

        [JsonPropertyName("total_installation_count")]
        public long TotalInstallationCount { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class SewerPerInstallation
    {
        [JsonPropertyName("values")]
        public SewerPerInstallationLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public SewerPerInstallationLastValue LastValue { get; set; }
    }

    public class SewerPerInstallationLastValue
    {
        [JsonPropertyName("date_measurement_unix")]
        public long DateMeasurementUnix { get; set; }

        [JsonPropertyName("week_start_unix")]
        public long WeekStartUnix { get; set; }

        [JsonPropertyName("week_end_unix")]
        public long WeekEndUnix { get; set; }

        [JsonPropertyName("rwzi_awzi_code")]
        public string RwziAwziCode { get; set; }

        [JsonPropertyName("rwzi_awzi_name")]
        public string RwziAwziName { get; set; }

        [JsonPropertyName("vrcode")]
        public string Vrcode { get; set; }

        [JsonPropertyName("vrnaam")]
        public string Vrnaam { get; set; }

        [JsonPropertyName("gm_code")]
        public string GmCode { get; set; }

        [JsonPropertyName("rna_normalized")]
        public double RnaNormalized { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }

    public class VerdenkingenHuisartsen
    {
        [JsonPropertyName("values")]
        public VerdenkingenHuisartsenLastValue[] Values { get; set; }

        [JsonPropertyName("last_value")]
        public VerdenkingenHuisartsenLastValue LastValue { get; set; }
    }

    public class VerdenkingenHuisartsenLastValue
    {
        [JsonPropertyName("week_unix")]
        public long WeekUnix { get; set; }

        [JsonPropertyName("week_start_unix")]
        public long WeekStartUnix { get; set; }

        [JsonPropertyName("week_end_unix")]
        public long WeekEndUnix { get; set; }

        [JsonPropertyName("incidentie")]
        public double Incidentie { get; set; }

        [JsonPropertyName("geschat_aantal")]
        public long GeschatAantal { get; set; }

        [JsonPropertyName("date_of_insertion_unix")]
        public long DateOfInsertionUnix { get; set; }
    }
}
