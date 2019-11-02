using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public class OrganizationItem
    {
        private string orgName;

        public string Name
        {
            get
            {
                return Name = orgName;
            }
            set
            {
                orgName = value;
            }
        }

        private int orgID;

        public int ID
        {
            get
            {
                return orgID;
            }

            set
            {
                orgID = value;
            }
        }
    }
    public class ProvinceItem
    {
        private string provinceName;

        public string Name
        {
            get
            {
                return provinceName;
            }
            set
            {
                provinceName = value;
            }
        }

        private string provinceCode;

        public string Code
        {
            get
            {
                return provinceCode;
            }
            set
            {
                provinceCode = value;
            }
        }

        private int provinceID;

        public int ID
        {
            get
            {
                return provinceID;
            }

            set
            {
                provinceID = value;
            }
        }
    }
    public class DistrictItem
    {
        private string districtName;

        public string Name
        {
            get
            {
                return Name = districtName;
            }
            set
            {
                districtName = value;
            }
        }

        private int provinceID;

        public int ProvinceID
        {
            get
            {
                return provinceID;
            }

            set
            {
                provinceID = value;
            }
        }

        private int districtID;

        public int ID
        {
            get
            {
                return districtID;
            }

            set
            {
                districtID = value;
            }
        }
    }
    public class LlgItem
    {
        private string llgName;

        public string Name
        {
            get
            {
                return Name = llgName;
            }
            set
            {
                llgName = value;
            }
        }

        private int districtID;

        public int DistrictID
        {
            get
            {
                return districtID;
            }

            set
            {
                districtID = value;
            }
        }

        private int llgID;

        public int ID
        {
            get
            {
                return llgID;
            }

            set
            {
                llgID = value;
            }
        }
    }

    public class WardnoItem
    {
        private string wardnoName;

        public string Name
        {
            get
            {
                return Name = wardnoName;
            }
            set
            {
                wardnoName = value;
            }
        }

        private int llgID;

        public int LlgID
        {
            get
            {
                return llgID;
            }

            set
            {
                llgID = value;
            }
        }

        private int wardnoID;

        public int ID
        {
            get
            {
                return wardnoID;
            }

            set
            {
                wardnoID = value;
            }
        }
    }
    public class IncidentItem
    {
        private string incidentName;

        public string Name
        {
            get
            {
                return Name = incidentName;
            }
            set
            {
                incidentName = value;
            }
        }

        private int incidentID;

        public int ID
        {
            get
            {
                return incidentID;
            }

            set
            {
                incidentID = value;
            }
        }
    }
    public class DisplacementItem
    {
        private string displacementName;

        public string Name
        {
            get
            {
                return Name = displacementName;
            }
            set
            {
                displacementName = value;
            }
        }

        private int displacementID;

        public int ID
        {
            get
            {
                return displacementID;
            }

            set
            {
                displacementID = value;
            }
        }
    }
    public class UnitItem
    {
        private string unitName;

        public string Name
        {
            get
            {
                return Name = unitName;
            }
            set
            {
                unitName = value;
            }
        }

        private int unitID;

        public int ID
        {
            get
            {
                return unitID;
            }

            set
            {
                unitID = value;
            }
        }
    }
    public class CauseItem
    {
        private string causeName;

        public string Name
        {
            get
            {
                return Name = causeName;
            }
            set
            {
                causeName = value;
            }
        }

        private int causeID;

        public int ID
        {
            get
            {
                return causeID;
            }

            set
            {
                causeID = value;
            }
        }
    }
    public class CountryItem
    {
        private string countryName;

        public string Name
        {
            get
            {
                return Name = countryName;
            }
            set
            {
                countryName = value;
            }
        }

        private int countryID;

        public int ID
        {
            get
            {
                return countryID;
            }

            set
            {
                countryID = value;
            }
        }
    }
    public class HouseholdItem
    {
        private string householdName;

        public string Name
        {
            get
            {
                return Name = householdName;
            }
            set
            {
                householdName = value;
            }
        }

        private int communityID;

        public int CommunityID
        {
            get
            {
                return communityID;
            }

            set
            {
                communityID = value;
            }
        }

        private int hoseholdID;

        public int ID
        {
            get
            {
                return hoseholdID;
            }

            set
            {
                hoseholdID = value;
            }
        }
    }
    public class CommunityItem
    {
        private string communityName;

        public string Name
        {
            get
            {
                return Name = communityName;
            }
            set
            {
                communityName = value;
            }
        }

        private int communityID;

        public int ID
        {
            get
            {
                return communityID;
            }

            set
            {
                communityID = value;
            }
        }
    }
    public class LevelofEducationItem
    {
        private string educationName;

        public string Name
        {
            get
            {
                return Name = educationName;
            }
            set
            {
                educationName = value;
            }
        }

        private int educationID;

        public int ID
        {
            get
            {
                return educationID;
            }

            set
            {
                educationID = value;
            }
        }
    }
    public class MaritalItem
    {
        private string maritalName;

        public string Name
        {
            get
            {
                return Name = maritalName;
            }
            set
            {
                maritalName = value;
            }
        }

        private int maritalID;

        public int ID
        {
            get
            {
                return maritalID;
            }

            set
            {
                maritalID = value;
            }
        }
    }
    public class OccupationItem
    {
        private string occupationName;

        public string Name
        {
            get
            {
                return Name = occupationName;
            }
            set
            {
                occupationName = value;
            }
        }

        private int occupationID;

        public int ID
        {
            get
            {
                return occupationID;
            }

            set
            {
                occupationID = value;
            }
        }
    }
    public class RoleItem
    {
        private string roleName;

        public string Name
        {
            get
            {
                return Name = roleName;
            }
            set
            {
                roleName = value;
            }
        }

        private int roleID;

        public int ID
        {
            get
            {
                return roleID;
            }

            set
            {
                roleID = value;
            }
        }
    }
    public class VulnerabilityItem
    {
        private string vulnerabilityName;

        public string Name
        {
            get
            {
                return Name = vulnerabilityName;
            }
            set
            {
                vulnerabilityName = value;
            }
        }

        private int vulID;

        public int ID
        {
            get
            {
                return vulID;
            }

            set
            {
                vulID = value;
            }
        }
    }
    public class IndividualItem
    {
        private int individualID;

        public int ID
        {
            get
            {
                return individualID;
            }
            set
            {
                individualID = value;
            }
        }

        private int household_id;
        public int HouseholdID
        {
            get
            {
                return household_id;
            }

            set
            {
                household_id = value;
            }
        }

        private string first_name;
        public string FirstName
        {
            get
            {
                return first_name;
            }

            set
            {
                first_name = value;
            }
        }

        private string middle_name;
        public string MiddleName
        {
            get
            {
                return middle_name;
            }

            set
            {
                middle_name = value;
            }
        }

        private string last_name;
        public string LastName
        {
            get
            {
                return last_name;
            }

            set
            {
                last_name = value;
            }
        }

        private int gender;
        public int Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        private string dob;
        public string DOB
        {
            get
            {
                return dob;
            }

            set
            {
                dob = value;
            }
        }

        private int age;
        public int Age
        {
            get
            {
                return age;
            }

            set
            {
                age = value;
            }
        }

        private int country_of_birth;
        public int CountyOfBirth
        {
            get
            {
                return country_of_birth;
            }

            set
            {
                country_of_birth = value;
            }
        }

        private string place_of_birth;
        public string PlaceOfBirth
        {
            get
            {
                return place_of_birth;
            }

            set
            {
                place_of_birth = value;
            }
        }

        private string nationality;
        public string Nationality
        {
            get
            {
                return nationality;
            }

            set
            {
                nationality = value;
            }
        }

        private string national_id;
        public string NationalID
        {
            get
            {
                return national_id;
            }

            set
            {
                national_id = value;
            }
        }

        private string contact_phone;
        public string ContactPhone
        {
            get
            {
                return contact_phone;
            }

            set
            {
                contact_phone = value;
            }
        }

        private string physical_address;
        public string PhysicalAddress
        {
            get
            {
                return physical_address;
            }

            set
            {
                physical_address = value;
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        private int marital_status;
        public int MaritalStatus
        {
            get
            {
                return marital_status;
            }

            set
            {
                marital_status = value;
            }
        }

        private int role;
        public int Role
        {
            get
            {
                return role;
            }

            set
            {
                role = value;
            }
        }

        private int is_bread_winner;
        public int IsBreadWinner
        {
            get
            {
                return is_bread_winner;
            }

            set
            {
                is_bread_winner = value;
            }
        }

        private int level_of_education;
        public int LevelOfEducation
        {
            get
            {
                return level_of_education;
            }

            set
            {
                level_of_education = value;
            }
        }

        private int occupation;
        public int Occupation
        {
            get
            {
                return occupation;
            }

            set
            {
                occupation = value;
            }
        }

        private string employer;
        public string Employer
        {
            get
            {
                return employer;
            }

            set
            {
                employer = value;
            }
        }

        private string comment;
        public string Comment
        {
            get
            {
                return comment;
            }

            set
            {
                comment = value;
            }
        }

        private int displacement_status;
        public int DisplacementStatus
        {
            get
            {
                return displacement_status;
            }

            set
            {
                displacement_status = value;
            }
        }

        private int vulnerability_level;
        public int VulnerabilityLevel
        {
            get
            {
                return vulnerability_level;
            }

            set
            {
                vulnerability_level = value;
            }
        }
    }
    public class UserItem
    {
        private string first_name;

        public string FirstName
        {
            get
            {
                return first_name;
            }
            set
            {
                first_name = value;
            }
        }

        private string last_name;

        public string LastName
        {
            get
            {
                return last_name;
            }
            set
            {
                last_name = value;
            }
        }

        private string user_name;

        public string UserName
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
            }
        }

        private int user_group;

        public int UserGroup
        {
            get
            {
                return user_group;
            }
            set
            {
                user_group = value;
            }
        }


        private int user_org;

        public int UserOrganization
        {
            get
            {
                return user_org;
            }
            set
            {
                user_org = value;
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        private int notify_new_case;
        public int NotifyNewCase
        {
            get
            {
                return notify_new_case;
            }
            set
            {
                notify_new_case = value;
            }
        }

        private int enabled;
        public int Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        private int locked;
        public int Locked
        {
            get
            {
                return locked;
            }
            set
            {
                locked = value;
            }
        }

        private int userID;

        public int ID
        {
            get
            {
                return userID;
            }

            set
            {
                userID = value;
            }
        }

        private string password;

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
    }

    public class MainInformation
    {
        public int Id { get; set; }
        public string OrganizationId { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string LlgId { get; set; }
        public string WardnoId { get; set; }
        public string PlaceName { get; set; }
        public DateTime IncidentOccured { get; set; }
        public DateTime IncidentReported { get; set; }
        public string CaseNo { get; set; }
    }

    public class AssistanceType
    {
        public int Id { get; set; }
        public string AssistanceTypeName { get; set; }
    }

    public class AssistanceModel
    {
        public int Id { get; set; }
        public string CaseNo { get; set; }
        public int AssistanceTypeId { get; set; }
        public string AssistanceDate { get; set; }
        public int Beneficiaries { get; set; }
        public string Comment { get; set; }
    }

    public class JsonResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}