using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Leaf.Model;

namespace Leaf.Migrations
{
    [DbContext(typeof(MyDBContext))]
    [Migration("20170201063602_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Leaf.Model.GapFilling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<int>("Level");

                    b.Property<string>("Stems");

                    b.Property<string>("Subject");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("GapFillings");
                });

            modelBuilder.Entity("Leaf.Model.SingleChoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<string>("Choices1");

                    b.Property<string>("Choices2");

                    b.Property<string>("Choices3");

                    b.Property<int>("Level");

                    b.Property<string>("Stems");

                    b.Property<string>("Subject");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("SingleChoices");
                });

            modelBuilder.Entity("Leaf.Model.TestPaper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuildTime");

                    b.Property<int>("GapNum");

                    b.Property<string>("GapQuestionNum");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.Property<int>("Score");

                    b.Property<int>("SingleNum");

                    b.Property<string>("SingleQuestionNum");

                    b.Property<int>("Time");

                    b.HasKey("Id");

                    b.ToTable("TestPapers");
                });

            modelBuilder.Entity("Leaf.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Admin");

                    b.Property<string>("BuildTime");

                    b.Property<string>("Password");

                    b.Property<string>("Score");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
        }
    }
}
