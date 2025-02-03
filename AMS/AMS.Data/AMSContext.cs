using AMS.DATA.Base;
using AMS.DOMAIN.Audit;
using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;
using AMS.DOMAIN.Entities.Lookups;
using AMS.DOMAIN.Identity;
using AMS.SHARED.Interfaces.CurrentUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace AMS.DATA
{
    public partial class AMSContext : BaseContext
    {
        private readonly ICurrentUser _currentUser;

        public AMSContext(DbContextOptions<AMSContext> options,

             ICurrentUser currentUser

            ) : base(options)
        {
            _currentUser = currentUser;
        }

        #region Domain
        public virtual DbSet<ApplicationForm> ApplicationForms { get; set; } = null!;
        public virtual DbSet<Applicant> Applicants { get; set; } = null!;
        public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; } = null!;
        public virtual DbSet<Guardian> Guardians { get; set; } = null!;
        public virtual DbSet<ApplicantDegree> ApplicantDegrees { get; set; } = null!;
        public virtual DbSet<FeeChallan> FeeChallans { get; set; } = null!;
        public virtual DbSet<FeeChallanSubmissionDetail> FeeChallanSubmissionDetails { get; set; } = null!;
        public virtual DbSet<ProgramApplied> ProgramsApplied { get; set; } = null!;
        public virtual DbSet<MeritList> MeritLists { get; set; } = null!;
        public virtual DbSet<MeritListDetails> MeritListDetails { get; set; } = null!;

        #endregion

        #region Lookups
        public virtual DbSet<AcademicYear> AcademicYears { get; set; } = null!;
        public virtual DbSet<Faculity> Faculties { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<ProgramDepartment> ProgramDepartments { get; set; } = null!;
        public virtual DbSet<Program> Programs { get; set; } = null!;
        public virtual DbSet<ProgramType> ProgramTypes { get; set; } = null!;
        public virtual DbSet<AdmissionSession> Sessions { get; set; } = null!;
        public virtual DbSet<DegreeGroup> DegreeGroups { get; set; } = null!;
        public virtual DbSet<DegreeLevel> DegreeLevels { get; set; } = null!;
        public virtual DbSet<TimeShift> TimeShifts { get; set; } = null!;
        public virtual DbSet<EntranceTestDetail> EntranceTests { get; set; } = null!;
        public virtual DbSet<TestType> TestTypes { get; set; } = null!;


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
                entity.Property(e => e.IsActive).HasDefaultValue(false);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.HasMany(u => u.UserRoles)
         .WithOne(ur => ur.User)
         .HasForeignKey(ur => ur.UserId)
         .IsRequired();

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
                entity.HasMany(r => r.UserRoles)
        .WithOne(ur => ur.Role)
        .HasForeignKey(ur => ur.RoleId)
        .IsRequired();

            });

            modelBuilder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable("UserRole", "Auth");
                entity.HasOne(ur => ur.User)
         .WithMany(u => u.UserRoles)
         .HasForeignKey(ur => ur.UserId)
         .IsRequired();

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .IsRequired();

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

            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.ToTable("Applicant", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Dob).HasColumnType("date")
                                           .HasColumnName("DOB");

                entity.Property(e => e.Cnic).HasMaxLength(13)
                                                    .HasColumnName("CNIC")
                                                    .IsUnicode(false);

                entity.Property(e => e.FatherName).HasMaxLength(20)
                                                    .IsUnicode(false);
                // unique constraint 
                entity.HasAlternateKey(e => e.Cnic);

                entity.Property(e => e.Gender).HasMaxLength(15)
                                              .IsUnicode(false);
                                              

                entity.Property(e => e.PostalAddress).HasMaxLength(100)
                                              .IsUnicode(false);

                entity.Property(e => e.PermanentAddress).HasMaxLength(100)
                                              .IsUnicode(false);

                entity.Property(e => e.PostalCode);

                entity.Property(e => e.Religion).HasMaxLength(30)
                                                     .IsUnicode(false);

                entity.Property(e => e.BloodGroup).HasMaxLength(5)
                                                  .IsUnicode(false);

                entity.Property(e => e.Domicile).HasMaxLength(20)
                                                  .IsUnicode(false);

                entity.Property(e => e.City).HasMaxLength(30)
                                                  .IsUnicode(false);

                entity.Property(e => e.Province)
                                                .HasMaxLength(20)
                                                .IsUnicode(false);

                entity.Property(e => e.HeardAboutUniFrom).HasMaxLength(20)
                                                         .IsUnicode(false);

                entity.Property(e => e.EmploymentDetails).HasMaxLength(50)
                                                         .IsUnicode(false)
                                                         .IsRequired(false);
                entity.HasOne(e => e.EmergencyContact)
                      .WithOne(e => e.Applicant)
                      .HasForeignKey<EmergencyContact>(e => e.ApplicantId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Applicant_EmergencyContact");

                entity.HasOne(e => e.Guardian)
                      .WithOne(e => e.Applicant)
                      .HasForeignKey<Guardian>(e => e.ApplicantId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Applicant_Guardian");

                entity.HasOne(e => e.EntranceTestDetail)
                      .WithOne(e => e.Applicant)
                      .HasForeignKey<EntranceTestDetail>(e => e.ApplicantId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Applicant_EntranceTestDetail");
               
                entity.HasOne(e => e.ApplicationForm)
                        .WithOne(e => e.Applicant)
                        .HasForeignKey<ApplicationForm>(e => e.ApplicantId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Applicant_ApplicationForm");

            });
            // Merit List Entities
            modelBuilder.Entity<MeritList>(entity => {
              
                entity.ToTable("MeritList", "Domain");

                entity.HasKey(e => e.Id);   

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Program)
                       .WithMany(e => e.MeritLists)
                       .HasForeignKey(e => e.ProgramId)
                       .OnDelete(DeleteBehavior.Cascade)
                       .HasConstraintName("FK_MeritList_Program");
                
                entity.HasOne(e => e.Session)
                       .WithMany(e => e.MeritLists)
                       .HasForeignKey(e => e.SessionId)
                       .OnDelete(DeleteBehavior.Cascade)
                       .HasConstraintName("FK_MeritList_Session");
                
                entity.HasOne(e => e.Department)
                       .WithMany(e => e.MeritLists)
                       .HasForeignKey(e => e.DepartmentId)
                       .OnDelete(DeleteBehavior.Cascade)
                       .HasConstraintName("FK_MeritList_Department");
                
                entity.HasOne(e => e.Shift)
                       .WithMany(e => e.MeritLists)
                       .HasForeignKey(e => e.ShiftId)
                       .OnDelete(DeleteBehavior.Cascade)
                       .HasConstraintName("FK_MeritList_Shift");

                entity.HasIndex(ml => new { ml.SessionId, ml.ProgramId, ml.DepartmentId, ml.ShiftId });


            });

            modelBuilder.Entity<MeritListDetails>(entity =>
            {
                entity.ToTable("MeritListDetails", "Domain");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
              
                entity.HasOne(e => e.MeritList)
                       .WithMany(e => e.MeritListDetails)
                       .HasForeignKey(e => e.MeritListId)
                       .OnDelete(DeleteBehavior.NoAction)
                       .HasConstraintName("FK_MeritListDetails_MeritList");

                entity.Property(x => x.Score).HasPrecision(18, 2);

            });

            modelBuilder.Entity<EmergencyContact>(entity =>
            {
                entity.ToTable("EmergencyContact", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ContactNo).HasMaxLength(11)
                                                  .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(30)
                                                  .IsUnicode(false);

                entity.Property(e => e.Relation).HasMaxLength(20)
                                                  .IsUnicode(false);


                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<Guardian>(entity =>
            {
                entity.ToTable("Guardian", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ContactNo).HasMaxLength(11)
                                                 .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(30)
                                            .IsUnicode(false);

                entity.Property(e => e.Relation).HasMaxLength(20)
                                                .IsUnicode(false);


                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<ApplicantDegree>(entity =>
            {
                entity.ToTable("ApplicantDegree", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BoardOrUniversityName).HasMaxLength(100)
                                                  .IsUnicode(false);

                entity.Property(e => e.Subject).HasMaxLength(30)
                                                  .IsUnicode(false);

                entity.Property(e => e.RollNo).HasMaxLength(15)
                                              .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(e => e.Applicant)
                      .WithMany(e => e.Degrees)
                      .HasForeignKey(e => e.ApplicantId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Degree_Applicant");

                entity.HasOne(e => e.DegreeGroup)
                      .WithMany(e => e.ApplicantDegrees)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Degree_DegreeGroup");
            });

            modelBuilder.Entity<ApplicationForm>(entity =>
            {
                entity.ToTable("ApplicationForm", "Domain");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.InfoConsent).HasDefaultValue(false);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.SubmissionDate).HasColumnType("datetime2");
                entity.Property(e => e.IsSubmitted).HasDefaultValue(false);

                entity.HasOne(e => e.MeritListDetails)
                      .WithOne(e => e.ApplicationForm)
                      .HasForeignKey<MeritListDetails>(e => e.ApplicationFormId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ApplicationForm_MeritListDetails");

                entity.HasOne(e => e.FeeChallan)
                        .WithOne(e => e.ApplicationForm)
                        .HasForeignKey<FeeChallan>(e => e.ApplicationFormId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ApplicationForm_FeeChallan");

                entity.HasOne(e => e.Program)
                      .WithMany(e => e.ApplicationForms)
                      .HasForeignKey(e => e.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ApplicationForms_Programs");
            });

            modelBuilder.Entity<FeeChallan>(entity =>
            {
                entity.ToTable("FeeChallan", "Domain");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DueTill).HasColumnType("datetime2");
                entity.Property(e => e.IssuedOn).HasColumnType("datetime2");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(e => e.FeeChallanSubmissionDetail)
                        .WithOne(e => e.FeeChallan)
                        .HasForeignKey<FeeChallanSubmissionDetail>(e => e.FeeChallanId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_FeeChallan_FeeChallanSubmissionDetail");
            });

            modelBuilder.Entity<FeeChallanSubmissionDetail>(entity =>
            {
                entity.ToTable("FeeChallanSubmissionDetail", "Domain");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
               
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.DocumentUrl)
                                    .IsUnicode(false);
                entity.Property(e => e.SubmissionDate).HasColumnType("datetime2");
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

                entity.HasOne(e => e.Department)
                    .WithMany(e => e.ProgramApplied)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProgramApplied_Department");


                entity.HasOne(e => e.TimeShift)
                    .WithMany(e => e.ProgramApplied)
                    .HasForeignKey(e => e.TimeShiftId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProgramApplied_TimeShift");
            });
            #endregion

            #region Lookups

            modelBuilder.Entity<EntranceTestDetail>(entity =>
            {
                entity.ToTable("EntranceTestDetail", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.TestValideTill).HasColumnType("datetime2");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(e => e.TestType)
                   .WithMany(e => e.EntranceTestDetails)
                   .HasForeignKey(e => e.TestTypeId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_EntranceTestDetail_TestType");

            });

            modelBuilder.Entity<TestType>(entity =>
            {
                entity.ToTable("TestType", "Domain");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(10);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<AcademicYear>(entity =>
            {
                entity.ToTable("AcademicYear", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.StartDate).HasColumnType("date");
                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.HasMany(e => e.Sessions)
                      .WithOne(e => e.AcademicYear)
                      .HasForeignKey(e => e.AcademicYearId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_AcademicYear_AdmissionSession");

                entity.Property(e => e.EndDate).HasColumnType("date");
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

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(e => e.ProgramType)
                      .WithMany(e => e.Programs)
                      .HasForeignKey(e => e.ProgramTypeId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProgramType_Programs");
            });

            modelBuilder.Entity<ProgramDepartment>(entity =>
            {
                entity.ToTable("ProgramDepartment", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(e => e.Faculity)
                      .WithMany(e => e.ProgramDepartments)
                      .HasForeignKey(e => e.FaculityId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .HasConstraintName("FK_ProgramDepartment_Faculty");
                
                entity.HasOne(e => e.Program)
                      .WithMany(e => e.ProgramDepartments)
                      .HasForeignKey(e => e.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProgramDepartment_Programs");

                entity.HasOne(e => e.Department)
                      .WithMany(e => e.ProgramDepartments)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_ProgramDepartment_Departments");

                entity.HasOne(e => e.TimeShift)
                      .WithMany(e => e.ProgramDepartments)
                      .HasForeignKey(e => e.TimeShiftId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ProgramDepartment_TimeShifts");
            });

            modelBuilder.Entity<TimeShift>(entity =>
            {
                entity.ToTable("TimeShift", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Faculity>(entity =>
            {
                entity.ToTable("Faculity", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasMaxLength(100)
                      .IsUnicode(false);

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

                entity.HasOne(e => e.Faculity)
             .WithMany(e => e.Departments)
             .OnDelete(DeleteBehavior.Cascade)
             .HasForeignKey(e => e.FaculityId)
             .HasConstraintName("FK_Department_Faculity");

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

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<DegreeGroup>(entity =>
            {
                entity.ToTable("DegreeGroup", "Lookup");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DegreeName)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.HasOne(e => e.DegreeLevel)
                      .WithMany(e => e.DegreeGroups)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasForeignKey(e => e.DegreeLevelId)
                      .HasConstraintName("FK_DegreeGroups_Faculity");

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

                entity.Property(e => e.StartDate).HasColumnType("date");
                entity.Property(e => e.EndDate).HasColumnType("date");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasMany(e => e.ApplicationForms)
                      .WithOne(e => e.Session)
                      .HasForeignKey(e => e.SessionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_SessionApplicationForms"
                      );
            });
            #endregion

        // soft deletes get query 
        modelBuilder.Entity<Faculity>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        modelBuilder.Entity<Applicant>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        modelBuilder.Entity<ApplicationForm>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        modelBuilder.Entity<Department>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        modelBuilder.Entity<Program>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        modelBuilder.Entity<ApplicationRole>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        modelBuilder.Entity<ProgramApplied>().HasQueryFilter(f => f.IsDeleted == false || f.IsDeleted == null);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}