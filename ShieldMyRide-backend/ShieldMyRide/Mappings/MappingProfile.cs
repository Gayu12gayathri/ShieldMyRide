using AutoMapper;
using ShieldMyRide.Authentication;
using ShieldMyRide.DTOs.ClaimDTO;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.DTOs.PolicyDTO;
using ShieldMyRide.DTOs.ProposalDTO;
using ShieldMyRide.DTOs.ProposalpolicyDocument;
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
            CreateMap<User, CustomerDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId.ToString()))  // convert int → string
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FirstName + "_" + src.LastName))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.AadhaarMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskAadhaar(src.AadhaarNumber)))
                .ForMember(dest => dest.PanMasked,
                    opt => opt.MapFrom(src => MaskingHelper.MaskPan(src.PanNumber)));

            // Officer Mapping (with masking)
            CreateMap<User, OfficerDeatilDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId.ToString())) // convert int → string
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FirstName + "_" + src.LastName))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.AadhaarMasked,
                        opt => opt.MapFrom(src => MaskingHelper.MaskAadhaar(src.AadhaarNumber)))
                    .ForMember(dest => dest.PanMasked,
                        opt => opt.MapFrom(src => MaskingHelper.MaskPan(src.PanNumber)));


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

            CreateMap<OfficerAssignmentDTO, OfficerAssignment>()
                .ForMember(dest => dest.OfficerAssignmentId, opt => opt.MapFrom(src => src.OfficerAssignmentId))
                .ForMember(dest => dest.OfficerId, opt => opt.MapFrom(src => src.OfficerId))
                .ForMember(dest => dest.ProposalId, opt => opt.MapFrom(src => src.ProposalId))
                .ForMember(dest => dest.ClaimId, opt => opt.MapFrom(src => src.ClaimId))
                .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<IFormFile, PolicyDocument>()
                .ForMember(dest => dest.DocumentType, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentPath, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<ProposalWithDocumentsFormModelDTO, Proposal>()
            .ForMember(dest => dest.ProposalStatus, opt => opt.MapFrom(src => ProposalStatus.Submitted))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Premium, opt => opt.Ignore());


        }
    }
}
