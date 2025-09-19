using AutoMapper;
using ShieldMyRide.Authentication;
using ShieldMyRide.DTOs.ClaimDTO;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.DTOs.PolicyDTO;
using ShieldMyRide.DTOs.ProposalDTO;
using ShieldMyRide.DTOs.QuoteDTO;
using ShieldMyRide.DTOs.UsersDTO;
using ShieldMyRide.Helpers;
using ShieldMyRide.Models;

namespace ShieldMyRide.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Admin Mapping
            CreateMap<ApplicationUser, AdminDTo>();

            // Customer Mapping (with masking)
            CreateMap<ApplicationUser, CustomerDTO>()
                .ForMember(dest => dest.AadhaarMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskAadhaar(src.AadhaarHash)))
                .ForMember(dest => dest.PanMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskPan(src.PanHash)));

            // Officer Mapping (with masking)
            CreateMap<ApplicationUser, OfficerDeatilDTO>()
                .ForMember(dest => dest.AadhaarMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskAadhaar(src.AadhaarHash)))
                .ForMember(dest => dest.PanMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskPan(src.PanHash)));

            //  OfficerAdminDTO
            CreateMap<OfficerAssignment, OfficerAdminDTO>()
                .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.OfficerAssignmentId))
                .ForMember(dest => dest.OfficerName, opt => opt.MapFrom(src => src.Officer.FirstName + " " + src.Officer.LastName))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Proposal.User.FirstName + " " + src.Proposal.User.LastName))
                .ForMember(dest => dest.VehicleRegNo, opt => opt.MapFrom(src => src.Proposal.VehicleRegNo));

            //  OfficerDTO
            CreateMap<OfficerAssignment, OfficerDTO>()
                .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.OfficerAssignmentId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Proposal.User.FirstName + " " + src.Proposal.User.LastName))
                .ForMember(dest => dest.VehicleRegNo, opt => opt.MapFrom(src => src.Proposal.VehicleRegNo));

            // In your AutoMapper profile (e.g., MappingProfile.cs)
            CreateMap<OfficerReviewDTO, Proposal>()
                .ForMember(dest => dest.ProposalStatus, opt => opt.MapFrom(src => src.ProposalStatus));


            // Proposal → ProposalDTO
            CreateMap<Proposal, ProposalDTO>()
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProposalId))
                            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType ?? "Unknown"))
                            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ProposalStatus.ToString()));



            // InsuranceClaim → ClaimGetDTO
            CreateMap<InsuranceClaim, ClaimGetDTO>();

            // Quote → QuoteGetDTO
            CreateMap<Quote, QuoteGetDTO>()
             .ForMember(dest => dest.ProposalIds,
                 opt => opt.MapFrom(src => src.Proposal != null
                     ? new List<int> { src.Proposal.ProposalId }
                     : new List<int>()))
             .ForMember(dest => dest.UserIds,
                 opt => opt.MapFrom(src => src.Proposal != null
                     ? new List<int> { src.Proposal.UserId }
                     : new List<int>()))
             .ForMember(dest => dest.Premium,
                 opt => opt.MapFrom(src => src.PremiumAmount))
             .ForMember(dest => dest.ValidTill,
                 opt => opt.MapFrom(src => src.ValidTill));

            // Policy → PolicyGetDTO
            CreateMap<Policy, PolicyGetDTO>()
                .ForMember(dest => dest.ProposalIds,
                    opt => opt.MapFrom(src => src.Proposals.Select(p => p.ProposalId)))
                .ForMember(dest => dest.Documents,
                    opt => opt.MapFrom(src => src.PolicyDocuments));

            CreateMap<PolicyDocument, PolicyDocumentDTO>();

            CreateMap<User, UserSearchDTO>()
                .ForMember(dest => dest.AadhaarMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskAadhaar(src.AadhaarNumber)))
                .ForMember(dest => dest.PanMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskPan(src.PanNumber)))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<OfficerAssignment, OfficerAssignmentDTO>()
                .ForMember(dest => dest.OfficerName,
                    opt => opt.MapFrom(src => src.Officer.FirstName + " " + src.Officer.LastName));


        }
    }
}
