﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRBD_Framework;

namespace prbd_2021_g01.Model
{
    public class EcoleContext : DbContextBase {

        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => {
            builder.AddConsole();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ecole;MultipleActiveResultSets=true")
                .EnableSensitiveDataLogging()
                //.UseLoggerFactory(_loggerFactory)
                .UseLazyLoadingProxies(true)
                ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // rendre l'email unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //// l'entité Member participe à une relation one-to-many ...
            //modelBuilder.Entity<Member>()
            //    // avec, du côté many, la propriété MessagesSent ...
            //    .HasMany(member => member.MessagesSent)
            //    // avec, du côté one, la propriété Author ...
            //    .WithOne(msg => msg.Author)
            //    // et pour laquelle on désactive le delete en cascade
            //    .OnDelete(DeleteBehavior.Restrict);

            //// l'entité Member participe à une relation one-to-many ...
            //modelBuilder.Entity<Member>()
            //    // avec, du côté many, la propriété MessagesReceived ...
            //    .HasMany(member => member.MessagesReceived)
            //    // avec, du côté one, la propriété Recipient ...
            //    .WithOne(msg => msg.Recipient)
            //    // et pour laquelle on désactive le delete en cascade
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        public void SeedData() {
            Database.BeginTransaction();

            //add data

            var bruno = new Teacher("Bruno","Lacroix","br@epfc.eu", "Password1,");
            //Teachers.AddRange(new[] { testTeacher }); // when we need to create many objects

            var benoit = new Teacher("Benoît", "Penelle", "be@epfc.eu", "Password1,");

            var boris = new Teacher("Boris", "Verhaegen", "bo@epfc.eu", "Password1,");

            var etudiant = new Student("Gakusei", "Sensei", "test@epfc.eu", "Password2,");

            var severine = new Student("Séverine", "Sensei", "se@epfc.eu", "Password2,");
            var sinouhe = new Student("Sinouhé", "Sensei", "si@epfc.eu", "Password2,");
            var ines = new Student("Ines", "Sensei", "in@epfc.eu", "Password2,");
            var celine = new Student("Céline", "Sensei", "ce@epfc.eu", "Password2,");

            var anc3 = new Course(bruno, "2002 - ANC3", 5, "Projet d'analyse et conception");
            var prbd = new Course(bruno, "2007 - PRBD", 5, "Projet de développement SGBD");
            var prwb = new Course(benoit, "1927 - PRWB", 5, "Projet de développement Web");
            var tgpr = new Course(benoit, "2000 - TGPR", 5, "Techniques de gestion de projets");
            var prm2 = new Course(boris, "5635 - PRM2", 5, "Principes algorithmiques et programmation");
            var pro2 = new Course(boris, "1995 - PRO2", 5, "Programmation orientée objets");

            var testDelete = new Course(bruno, "2002 - testDelete", 5, "testDelete");

            var firstRegistration = new Registration(etudiant, anc3, RegistrationState.Active);
            var secondRegistration = new Registration(etudiant, prbd, RegistrationState.Pending);

            var thirdRegistration = new Registration(ines, prbd, RegistrationState.Inactive); //à priori, n'était pas nécessaire car on avait dit que si on ne trouvait pas l'enregistrement, c'est que l'étudiant est inactif pour ce cours
            var fourthRegistration = new Registration(ines, anc3, RegistrationState.Pending);
            var fifthRegistration = new Registration(severine, prbd, RegistrationState.Inactive);

            var registration6 = new Registration(sinouhe, prbd, RegistrationState.Active);

            //cours anc3
            var analyse = new Category(anc3, "analyse");
            var prog = new Category(anc3, "programmation");
            var quest1 = new Question(anc3, "Q1");
            analyse.addQuestion(quest1);
            prog.addQuestion(quest1);
            var quest2 = new Question(anc3, "Q2");
            prog.addQuestion(quest2);
            var quest3 = new Question(anc3, "Q3");
            analyse.addQuestion(quest3);
            var ans1q1 = new Answer(quest1, "rep1q1_false", false);
            var ans2q1 = new Answer(quest1, "rep2q1_true", true);
            var ans3q1 = new Answer(quest1, "rep3q1_true", true);
            var ans1q2 = new Answer(quest2, "rep1q2_true", true);
            var ans2q2 = new Answer(quest2, "rep2q2_false", false);

            var quiz1 = new Quiz(anc3, "quiz1", new DateTime(2021,05,27), new DateTime(2021, 06, 30));
            var quiz2 = new Quiz(anc3, "quiz2", new DateTime(2021, 05, 10), new DateTime(2021, 05, 15));
            var quiz3 = new Quiz(anc3, "quiz3", new DateTime(2021, 06, 01), new DateTime(2021, 06, 05));

            //cours prbd
            var analysePRBD = new Category(prbd, "analysePRBD");
            var progPRBD = new Category(prbd, "programmationPRBD");
            var quest1PRBD = new Question(prbd, "Q1");
            analysePRBD.addQuestion(quest1PRBD);
            progPRBD.addQuestion(quest1PRBD);
            var quest2PRBD = new Question(prbd, "Q2");
            progPRBD.addQuestion(quest2PRBD);
            var quest3PRBD = new Question(prbd, "Q3");



            //bruno.AddCourse(anc3); 
            Courses.AddRange(anc3, prbd, prwb, tgpr, prm2, pro2, testDelete);

            Users.AddRange(bruno, benoit, boris, etudiant, severine, ines, sinouhe, celine);
            Categories.AddRange(analyse, prog, analysePRBD, progPRBD);
            Questions.AddRange(quest1, quest2, quest3, quest1PRBD, quest2PRBD, quest3PRBD);
            Registrations.AddRange(firstRegistration, secondRegistration, thirdRegistration, fourthRegistration, fifthRegistration, registration6);
            Answers.AddRange(ans1q1, ans2q1, ans3q1, ans1q2, ans2q2);
            Quizz.AddRange(quiz1, quiz2, quiz3);

            SaveChanges();

            Database.CommitTransaction();
        }

       
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Quiz> Quizz { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerStudent> AnswerStudents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }

    }
}