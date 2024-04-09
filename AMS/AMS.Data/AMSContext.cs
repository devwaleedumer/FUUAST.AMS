using AMS.DATA.Base;
using AMS.DOMAIN.Audit;
using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;
using AMS.DOMAIN.Entities.Lookups;
using AMS.DOMAIN.Identity;
using AMS.SHARED.Interfaces.CurrentUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AMS.DATA
{
    public partial class AMSContext :  BaseContext
    {
        private readonly ICurrentUser _currentUser;

        public AMSContext(DbContextOptions<AMSContext> options,

             ICurrentUser currentUser

            ) : base(options)
        {
            _currentUser = currentUser;
        }

        #region Domain
        public DbSet<ApplicationForm> ApplicationForms { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<ParentInfo> ParentInfos { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<ApplicantDegree> Degrees { get; set; }
        public DbSet<FeeChallan> FeeChallans { get; set; }
        public DbSet<FeeChallanSubmissionDetail> FeeChallanSubmissionDetails { get; set; }
        public DbSet<ProgramApplied> ProgramsApplied { get; set; }
        #endregion

        #region Lookups
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProgramType> ProgramTypes{ get; set; }
        public DbSet<AdmissionSession> Session { get; set; }
        public DbSet<PreviousDegreeDetail> PreviousDegreeDetails { get; set; }
        public DbSet<DegreeLevel> DegreeLevel { get; set; }

        #endregion

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            int? userId = _currentUser.GetUserId();
            foreach (var entry in ChangeTracker.Entries<IBaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.InsertedDate = DateTime.Now;
                        entry.Entity.InsertedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.UpdatedBy = userId;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.UpdatedBy = userId;
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        break;

                }

            }
            if (userId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(userId, cancellationToken);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Audit
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.ToTable(name: "Audit", "Auth");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            #endregion

            #region Identity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User", "Auth");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");


            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Role", "Auth");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Description).HasMaxLength(100);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasMany(e => e.RoleClaims)
                      .WithOne(e => e.Role)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_RoleClaims_Role");
            });

            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRole", "Auth");
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaim", "Auth");
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogin", "Auth");
            });

            modelBuilder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable(name: "UserRoleClaim", "Auth");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserToken", "Auth");
            });

            #endregion
           
            #region Domain

            modelBuilder.Entity<Applicant>((entity =>
            {
                entity.ToTable("Applicant", "Domain");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();


                entity.Property(e => e.Dob).HasColumnType("datetime")
                                                    .HasColumnName("DOB");
               
                entity.Property(e => e.WhatsappNo).HasMaxLength(15)
                                                    .IsUnicode(false);

                entity.Property(e => e.Cnic).HasMaxLength(15)
                                                    .HasColumnName("CNIC")
                                                    .IsUnicode(false);
                
                entity.Property(e => e.Gender).HasMaxLength(50)
                                                     .IsUnicode(false);

                entity.Property(e => e.Religion).HasMaxLength(50)
                                                     .IsUnicode(false);

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.DisablitityDetails).HasMaxLength(200);


                entity.Property(e => e.BloodGroup).HasMaxLength(50)
                                                  .IsUnicode(false);

                entity.Property(e => e.DomicileDistrict).HasMaxLength(50)
                                                  .IsUnicode(false);

                entity.Property(e => e.DomicileProvince).HasMaxLength(50)
                                                  .IsUnicode(false);
                
                entity.Property(e => e.NextOfKinName).HasMaxLength(50)
                                                  .IsUnicode(false);

                entity.Property(e => e.NextOfKinRelation).HasMaxLength(50)
                                                  .IsUnicode(false);

                entity.HasOne(e => e.ContactInfo)
                      .WithOne(e => e.Applicant)
                      .HasForeignKey<EmergencyContact>(e => e.ApplicantId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("Fk_Applicant_EmergencyContact");



                entity.HasOne(e => e.Guardian)
                      .WithOne(e => e.Applicant)
                      .HasForeignKey<Guardian>(e => e.ApplicantId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("Fk_Applicant_Guardian");

                entity.HasOne(e => e.ParentInfo)
                     .WithOne(e => e.Applicant)
                     .HasForeignKey<ParentInfo>(e => e.ApplicantId)
                     .OnDelete(DeleteBehavior.Cascade)
                     .HasConstraintName("Fk_Applicant_ParentInfo");

                entity.HasOne(e => e.ApplicationForm)
                    .WithOne(e => e.Applicant)
                    .HasForeignKey<ApplicationForm>(e => e.ApplicantId)
                    .HasConstraintName("FK_Applicant_ApplicationForm");


            }));

            modelBuilder.Entity<EmergencyContact>(entity =>
            {
                entity.ToTable("EmergencyContact", "Domain");
                
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
               
                entity.Property(e => e.ContactNO).HasMaxLength(15)
                                                  .IsUnicode(false);


                entity.Property(e => e.Name).HasMaxLength(50)
                                                  .IsUnicode(false);


                entity.Property(e => e.Relation).HasMaxLength(50)
                                                  .IsUnicode(false);


                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<Guardian>(entity =>
            {
                entity.ToTable("Guardian", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(50)
                                                  .IsUnicode(false);


                entity.Property(e => e.Occupation).HasMaxLength(50)
                                                  .IsUnicode(false);


                entity.Property(e => e.PhoneNo).HasMaxLength(15)
                                                  .IsUnicode(false);


                entity.Property(e => e.TotalPerMonthExpenses).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TotalPerMonthIncome).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<ApplicantDegree>(entity =>
            {
                entity.ToTable("ApplicantDegree", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BorardOrUniversityName).HasMaxLength(200)
                                                  .IsUnicode(false);

                 entity.Property(e => e.InstituteName).HasMaxLength(200)
                                                  .IsUnicode(false);

                entity.Property(e => e.MajorSubject).HasMaxLength(50)
                                                  .IsUnicode(false);

                entity.Property(e => e.ObtainedMarks).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.TotalMarks).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Percentage).HasColumnType("decimal(6, 2)");
                
                entity.Property(e => e.FromYear).HasColumnType("datetime");

                entity.Property(e => e.RollNo).HasMaxLength(50);
                
                entity.Property(e => e.ToYear).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(e => e.Applicant)
                      .WithMany(e => e.Degrees)
                      .HasForeignKey(e => e.ApplicantId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Degree_Applicant");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Province).HasMaxLength(50)
                                                .IsUnicode(false);

                entity.Property(e => e.District).HasMaxLength(50)
                                                .IsUnicode(false);

                entity.Property(e => e.StreetAddress).HasMaxLength(200)
                                                .IsUnicode(false);

                entity.HasOne(e => e.Applicant).WithMany(e => e.Addresses)
                                               .HasForeignKey(e => e.ApplicantId)
                                               .HasConstraintName("FK_Address_Applicant");
            });

            modelBuilder.Entity<ApplicationForm>(entity =>
            {
                entity.ToTable("ApplicationForm", "Domain");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.InfoConsent).HasDefaultValueSql("((0)");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0)");
                entity.Property(e => e.SubmissionDate).HasColumnType("datetime");
                entity.Property(e => e.IsSubmitted).HasDefaultValueSql("((0)");

                entity.HasOne(e => e.FeeChallan)
                        .WithOne(e => e.ApplicationForm)
                        .HasForeignKey<FeeChallan>(e => e.ApplicationFormId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ApplicationForm_FeeChallan");
            });

            modelBuilder.Entity<FeeChallan>(entity =>
            {
                entity.ToTable("FeeChallan", "Domain");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DueTill).HasColumnType("datetime");
                entity.Property(e => e.IssuedOn).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(e => e.FeeChallanSubmissionDetail)
                        .WithOne(e => e.FeeChallan)
                        .HasForeignKey<FeeChallanSubmissionDetail>(e => e.FeeChallanId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_FeeChallan_FeeChallanSubmissionDetail");
            });

            modelBuilder.Entity<FeeChallanSubmissionDetail>(entity => {
                entity.ToTable("FeeChallanSubmissionDetail", "Domain");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.BranchNameWithCity)
                                     .HasMaxLength(200)
                                    .IsUnicode(false);
                entity.Property(e => e.IsSubmitted).HasDefaultValueSql("((0))");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.DocumentUrl)
                                    .IsUnicode(false);
                entity.Property(e => e.SubmissionDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Guardian>(entity => {
               
                entity.ToTable("Guardian", "Domain");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(50)
                                            .IsUnicode(false);
                entity.Property(e => e.Relation).HasMaxLength(50)
                                            .IsUnicode(false);
                entity.Property(e => e.PhoneNo).HasMaxLength(15)
                                             .IsUnicode(false);
                entity.Property(e => e.TotalPerMonthExpenses).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TotalPerMonthIncome).HasColumnType("decimal(10,2)");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");


            });

            modelBuilder.Entity<ParentInfo>(entity => {
                entity.ToTable("ParentInfo", "Domain");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.FatherName).HasMaxLength(50)
                                            .IsUnicode(false);
                entity.Property(e => e.MotherName).HasMaxLength(50)
                                            .IsUnicode(false);
                entity.Property(e => e.FatherContact).HasMaxLength(15)
                                            .IsUnicode(false);
                entity.Property(e => e.FatherCNIC).HasMaxLength(20)
                                            .IsUnicode(false);
                entity.Property(e => e.FatherOccupation).HasMaxLength(50)
                                            .IsUnicode(false);
                entity.Property(e => e.IsFatherDeceased).HasDefaultValueSql("((0))");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<ProgramApplied>(entity =>
            {

                entity.ToTable("ProgramApplied", "Domain");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");


                entity.HasOne(e => e.ApplicationForm)
                      .WithMany(e => e.ProgramsApplied)
                      .HasForeignKey(e => e.ApplicationFormId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProgramApplied_ApplicationForm");

                

            });
            #endregion

            #region LookUps
            modelBuilder.Entity<AcademicYear>(entity =>
            {
                entity.ToTable("AcademicYear", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.HasMany(e => e.Sessions)
                      .WithOne(e => e.AcademicYear)
                      .HasForeignKey(e => e.AcademicYearId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_AcademicYear_AdmissionSession");


                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Program>(entity =>
            {
                entity.ToTable("Program", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

              entity.HasOne(e => e.ProgramApplied)
             .WithOne(e => e.Program)
             .HasForeignKey<ProgramApplied>(e => e.ProgramId)
             .HasConstraintName("FK_ProgramApplied_Program");

                entity.Property(e => e.Duration).HasColumnType("decimal(2,1)");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.HasMany(e => e.Programs)
               .WithOne(e => e.Department)
               .HasForeignKey(e => e.DepartmentId)
               .HasConstraintName("FK_Department_Program");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<DegreeLevel>(entity =>
            {
                entity.ToTable("DegreeLevel", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.HasMany(e => e.PreviousDegreeDetails)
               .WithOne(e => e.DegreeLevel)
               .HasForeignKey(e => e.DegreeLevelId)
               .HasConstraintName("FK_DegreeLevel_PreviousDegreeDetails");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<PreviousDegreeDetail>(entity =>
            {
                entity.ToTable("PreviousDegreeDetail", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DegreeName)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);


                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ProgramType>(entity =>
            {
                entity.ToTable("ProgramType", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasMany(e => e.Programs)
                       .WithOne(e => e.ProgramType)
                       .HasForeignKey(e => e.ProgramTypeId)
                       .OnDelete(DeleteBehavior.Cascade)
                       .HasConstraintName("FK_ProgramType_Programs");
            });

            modelBuilder.Entity<AdmissionSession>(entity =>
            {
                entity.ToTable("AdmissionSession", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasMany(e => e.ApplicationForms)
                      .WithOne(e => e.Session)
                      .HasForeignKey(e => e.SessionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_SessionApplicationForms");
             });
            OnModelCreatingPartial(modelBuilder);


            #endregion

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(wa => wa.Ignore(RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables));

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
