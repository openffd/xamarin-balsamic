using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    public class Availability : AvailabilityRawData
    {
        public bool includeFutureTerritories
        {
            get { return theWorld; }
            set { theWorld = value; }
        }
        public bool clearedForPreOrder
        {
            get { return preOrder.clearedForPreOrder.value; }
            set { preOrder.clearedForPreOrder.value = value; }
        }
        public string appAvailableDate
        {
            get { return preOrder.appAvailableDate.value; }
            set { preOrder.appAvailableDate.value = value; }
        }
        public bool b2bUnavailable
        {
            get { return b2BAppFlagDisabled; }
            set { b2BAppFlagDisabled = value; }
        }

        private List<Territory> _territories = new List<Territory>();
        public List<Territory> territories
        {
            get
            {
                if (_territories.Count > 0)
                    return _territories;

                _territories = countries.Select(country => JObject.FromObject(country).ToObject<Territory>()).ToList();
                return _territories;
            }

            set
            {
                _territories = value;
            }
        }

        private List<B2bUserValue> _b2bUsers = new List<B2bUserValue>();
        public List<B2bUserValue> convertedB2bUsers
        {
            get
            {
                if (_b2bUsers.Count > 0)
                    return _b2bUsers;

                if (b2bUsers != null)
                    _b2bUsers = b2bUsers.Select(b2bUser => b2bUser.value).ToList();

                return _b2bUsers;
            }
        }

        private List<B2bOrganizationValue> _b2bOrganizationValues = new List<B2bOrganizationValue>();
        public List<B2bOrganizationValue> convertedB2bOrganizationValues
        {
            get
            {
                if (_b2bOrganizationValues.Count > 0)
                    return _b2bOrganizationValues;

                if (b2bOrganizations != null)
                    _b2bOrganizationValues = b2bOrganizations.Select(b2bOrganization => b2bOrganization.value).ToList();

                return _b2bOrganizationValues;
            }
        }

        public static Availability FromTerritories(string[] territories = null, dynamic param = null)
        {
            // Initializes the DataHash with our preOrder structure so values
            // that are being modified will be saved
            //
            // Note: A better solution for this in the future might be to improve how
            // Base::DataHash sets values for paths that don't exist
            Availability obj = new Availability
            {
                preOrder = new PreOrder
                {
                    clearedForPreOrder = new ClearedForPreOrder
                    {
                        value = false
                    },
                    appAvailableDate = new AppAvailableDate
                    {
                        value = null
                    }
                }
            };

            obj.territories = territories.ToList().Select(territory => Territory.FromCode(territory)).ToList();
            obj.includeFutureTerritories = param != null && param.includeFutureTerritories ? param.includeFutureTerritories : true;
            obj.clearedForPreOrder = param != null && param.clearedForPreOrder ? param.clearedForPreOrder : false;
            obj.appAvailableDate = param != null && param.appAvailableDate ? param.appAvailableDate : null;
            obj.b2bUnavailable = param != null && param.b2bUnavailable ? param.b2bUnavailable : false;
            obj.b2bAppEnabled = param != null && param.b2bAppEnabled ? param.b2bAppEnabled : false;
            obj.educationalDiscount = true;

            return obj;
        }

        public static Availability FromTerritories(List<Territory> territories = null, dynamic param = null)
        {
            // Initializes the DataHash with our preOrder structure so values
            // that are being modified will be saved
            //
            // Note: A better solution for this in the future might be to improve how
            // Base::DataHash sets values for paths that don't exist
            Availability obj = new Availability
            {
                preOrder = new PreOrder
                {
                    clearedForPreOrder = new ClearedForPreOrder
                    {
                        value = false
                    },
                    appAvailableDate = new AppAvailableDate
                    {
                        value = null
                    }
                }
            };

            obj.territories = territories;
            obj.includeFutureTerritories = param != null && param.includeFutureTerritories ? param.includeFutureTerritories : true;
            obj.clearedForPreOrder = param != null && param.clearedForPreOrder ? param.clearedForPreOrder : false;
            obj.appAvailableDate = param != null && param.appAvailableDate ? param.appAvailableDate : null;
            obj.b2bUnavailable = param != null && param.b2bUnavailable ? param.b2bUnavailable : false;
            obj.b2bAppEnabled = param != null && param.b2bAppEnabled ? param.b2bAppEnabled : false;
            obj.educationalDiscount = true;

            return obj;
        }
    }

    public class AvailabilityRawData
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public bool countriesChanged { get; set; }
        public List<CountryPricing> countries { get; set; }
        public bool b2BAppFlagDisabled { get; set; }
        public bool educationDiscountDisabledFlag { get; set; }
        public bool hotFudgeFeatureEnabled { get; set; }
        public bool coldFudgeFeatureEnabled { get; set; }
        public bool bitcodeAutoRecompileDisAllowed { get; set; }
        public bool b2bAppEnabled { get; set; }
        public bool educationalDiscount { get; set; }
        public bool theWorld { get; set; }
        public PricingIntervalsFieldTO pricingIntervalsFieldTO { get; set; }
        public long availableDate { get; set; }
        public int unavailableDate { get; set; }
        public bool creatingApp { get; set; }
        public bool hasApprovedVersion { get; set; }
        public PreOrder preOrder { get; set; }
        public AppVersionsForOTAByPlatforms appVersionsForOTAByPlatforms { get; set; }
        public List<B2bUser> b2bUsers { get; set; }
        public List<B2bOrganization> b2bOrganizations { get; set; }
    }
}
