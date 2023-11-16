using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class ReinitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    major_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    avatar_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    role = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    is_awarded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MajorSubject",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    major_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorSubject", x => new { x.subject_id, x.major_id });
                    table.ForeignKey(
                        name: "FKMajorSu484641",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKMajorSu611140",
                        column: x => x.major_id,
                        principalTable: "Major",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FollowUser",
                columns: table => new
                {
                    follower_id = table.Column<int>(type: "int", nullable: false),
                    followed_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUser", x => new { x.follower_id, x.followed_id });
                    table.ForeignKey(
                        name: "FKFollowUser200833",
                        column: x => x.follower_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKFollowUser783758",
                        column: x => x.followed_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    reviewer_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_approved = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.id);
                    table.ForeignKey(
                        name: "FKPost548405",
                        column: x => x.reviewer_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKPost990072",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaveList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveList", x => x.id);
                    table.ForeignKey(
                        name: "FKSaveList283598",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMajor",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    major_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMajor", x => new { x.user_id, x.major_id });
                    table.ForeignKey(
                        name: "FKUserCatego136719",
                        column: x => x.major_id,
                        principalTable: "Major",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKUserCatego519402",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSubject",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubject", x => new { x.user_id, x.subject_id });
                    table.ForeignKey(
                        name: "FKUserSubject136719",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKUserSubject519402",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.id);
                    table.ForeignKey(
                        name: "FKComment400844",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKComment632288",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.id);
                    table.ForeignKey(
                        name: "FKMedia400844",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostMajor",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false),
                    major_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMajor", x => new { x.post_id, x.major_id });
                    table.ForeignKey(
                        name: "FKPostCatego136719",
                        column: x => x.major_id,
                        principalTable: "Major",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKPostCatego519402",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostSubject",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSubject", x => new { x.post_id, x.subject_id });
                    table.ForeignKey(
                        name: "FKPostSubject277175",
                        column: x => x.subject_id,
                        principalTable: "Subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKPostSubject716835",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportPost",
                columns: table => new
                {
                    reporter_id = table.Column<int>(type: "int", nullable: false),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    admin_id = table.Column<int>(type: "int", nullable: true),
                    content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPost", x => new { x.reporter_id, x.post_id });
                    table.ForeignKey(
                        name: "FKReportPost680973",
                        column: x => x.reporter_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKReportPost712211",
                        column: x => x.admin_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKReportPost960109",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VotePost",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    vote = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotePost", x => new { x.user_id, x.post_id });
                    table.ForeignKey(
                        name: "FKVotePost207593",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKVotePost439037",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostList",
                columns: table => new
                {
                    save_list_id = table.Column<int>(type: "int", nullable: false),
                    save_post_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostList", x => new { x.save_list_id, x.save_post_id });
                    table.ForeignKey(
                        name: "FKPostList365782",
                        column: x => x.save_list_id,
                        principalTable: "SaveList",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKPostList86128",
                        column: x => x.save_post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VoteComment",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    comment_id = table.Column<int>(type: "int", nullable: false),
                    vote = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteComment", x => new { x.user_id, x.comment_id });
                    table.ForeignKey(
                        name: "FKVoteCommen692680",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKVoteCommen971268",
                        column: x => x.comment_id,
                        principalTable: "Comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_post_id",
                table: "Comment",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_user_id",
                table: "Comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUser_followed_id",
                table: "FollowUser",
                column: "followed_id");

            migrationBuilder.CreateIndex(
                name: "IX_Major_major_name",
                table: "Major",
                column: "major_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MajorSubject_major_id",
                table: "MajorSubject",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_Media_post_id",
                table: "Media",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_reviewer_id",
                table: "Post",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_user_id",
                table: "Post",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostList_save_post_id",
                table: "PostList",
                column: "save_post_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostMajor_major_id",
                table: "PostMajor",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostSubject_subject_id",
                table: "PostSubject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPost_admin_id",
                table: "ReportPost",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPost_post_id",
                table: "ReportPost",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_SaveList_user_id",
                table: "SaveList",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_subject_name",
                table: "Subject",
                column: "subject_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__AB6E6164D120EBFA",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMajor_major_id",
                table: "UserMajor",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubject_subject_id",
                table: "UserSubject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_VoteComment_comment_id",
                table: "VoteComment",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_VotePost_post_id",
                table: "VotePost",
                column: "post_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUser");

            migrationBuilder.DropTable(
                name: "MajorSubject");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "PostList");

            migrationBuilder.DropTable(
                name: "PostMajor");

            migrationBuilder.DropTable(
                name: "PostSubject");

            migrationBuilder.DropTable(
                name: "ReportPost");

            migrationBuilder.DropTable(
                name: "UserMajor");

            migrationBuilder.DropTable(
                name: "UserSubject");

            migrationBuilder.DropTable(
                name: "VoteComment");

            migrationBuilder.DropTable(
                name: "VotePost");

            migrationBuilder.DropTable(
                name: "SaveList");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
