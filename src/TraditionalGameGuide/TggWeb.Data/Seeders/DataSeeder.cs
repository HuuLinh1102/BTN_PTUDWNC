using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TggWeb.Core.Entities;
using TggWeb.Data.Contexts;

namespace TggWeb.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly WebDbContext _dbConText;

        public DataSeeder(WebDbContext dbConText)
        {
            _dbConText = dbConText;
        }

        public void Initialize()
        {
            _dbConText.Database.EnsureCreated();

            if (_dbConText.Posts.Any()) return;

            var categories = AddCategories();
			var games = AddGames(categories);
			var tags = AddTags();
            var posts = AddPosts(games, tags);
        }

        

        private IList<Category> AddCategories() 
        {
            var categories = new List<Category>()
            {
                new() {Name = "Trí tuệ", UrlSlug = "tri-tue",
                    Description = "Trí tuệ"},
                new() {Name = "Thể thao", UrlSlug = "the-thao",
                    Description = "Thể thao" },
				new() {Name = "Đấu thú", UrlSlug = "dau-thu",
					Description = "Đấu thú" },
				new() {Name = "Trò chơi nhóm", UrlSlug = "tro-choi-nhom",
					Description = "Trò chơi nhóm" },
				new() {Name = "Cá cược", UrlSlug = "ca-cuoc",
					Description = "Cá cược" }

			};

            _dbConText.AddRange(categories);
            _dbConText.SaveChanges();

            return categories;
        
        }

		private IList<Game> AddGames(IList<Category> categories)
		{
			var games = new List<Game>()
			{
				new()
				{
					Name = "Ô ăn quan",
                    UrlSlug = "o-an-quan",
                    Description = "Trò chơi đối kháng trên bàn cờ với 7 lỗ cho mỗi người chơi",
                    Age = 3,
                    PlayerCount = 2,
                    Category = categories[0]
				},
				new()
				{
					Name = "Cờ cá ngựa",
					UrlSlug = "co-ca-ngua",
                    Description = "Trò chơi đua ngựa bằng cách ném xúc xắc, tránh chướng ngại vật",
                    Age = 3,
                    PlayerCount = 2,
					Category = categories[0]
				},
                new()
				{
					Name = "Cờ tướng",
					UrlSlug = "co-tuong",
					Description = "Trò chơi hai bên, tìm cách chiếu tướng đối phương",
					Age = 10,
					PlayerCount = 2,
					Category = categories[0]
				},
				new()
				{
					Name = "Cờ vua",
					UrlSlug = "co-vua",
					Description = "Trò chơi hai bên, tìm cách chiếu Vua đối phương",
					Age = 10,
					PlayerCount = 2,
					Category = categories[0]
				},
                new()
				{
					Name = "Đố vui",
					UrlSlug = "do-vui",
					Description = "Đố những câu hỏi vui, hỏi mẹo",
					Age = 5,
					PlayerCount = 2,
					Category = categories[0]
				},
				new()
				{
					Name = "Đá cầu",
					UrlSlug = "da-cau",
					Description = "Đá cầu là một môn thể thao phổ biến ở Việt Nam.",
					Age = 5,
					PlayerCount = 2,
					Category = categories[1]
				},
				new()
				{
					Name = "Kéo co",
					UrlSlug = "keo-co",
					Description = "Đá cầu là một môn thể thao phổ biến ở Việt Nam.",
					Age = 5,
					PlayerCount = 2,
					Category = categories[1]
				},
				new()
				{
					Name = "Nhảy dây",
					UrlSlug = "nhay-day",
					Description = "Nhảy dây là một môn thể thao phổ biến ở Việt Nam.",
					Age = 10,
					PlayerCount = 1,
					Category = categories[1]
				},
				new()
				{
					Name = "Trọi gà",
					UrlSlug = "troi-ga",
					Description = "Trọi gà",
					Age = 16,
					PlayerCount = 2,
					Category = categories[2]
				},
				new()
				{
					Name = "Trọi cá",
					UrlSlug = "troi-ca",
					Description = "Trọi cá",
					Age = 16,
					PlayerCount = 2,
					Category = categories[2]
				},
				new()
				{
					Name = "Trọi kiến",
					UrlSlug = "troi-kien",
					Description = "Trọi kiến",
					Age = 16,
					PlayerCount = 2,
					Category = categories[2]
				},
				new()
				{
					Name = "Trốn tìm",
					UrlSlug = "tron-tim",
					Description = "Trốn tìm là trò chơi gắn liền với tuổi thơ",
					Age = 3,
					PlayerCount = 4,
					Category = categories[3]
				},
				new()
				{
					Name = "Rồng rắn lên mây",
					UrlSlug = "rong-ran-len-may",
					Description = "Rồng rắn lên mây là trò chơi gắn liền với tuổi thơ",
					Age = 3,
					PlayerCount = 4,
					Category = categories[3]
				},
				new()
				{
					Name = "Bầu cua",
					UrlSlug = "bau-cua",
					Description = "Bầu cua tôm cá",
					Age = 16,
					PlayerCount = 4,
					Category = categories[4]
				},
				new()
				{
					Name = "Oẳn tù xì",
					UrlSlug = "oan-tu-xi",
					Description = "Oẳn tù xì hay kéo búa bao",
					Age = 3,
					PlayerCount = 2,
					Category = categories[4]
				},
				new()
				{
					Name = "Chắn",
					UrlSlug = "chan",
					Description = "Chắn giống như đánh bài vậy",
					Age = 16,
					PlayerCount = 4,
					Category = categories[4]
				}

			};

			_dbConText.Games.AddRange(games);
			_dbConText.SaveChanges();

			return games;

		}

		private IList<Tag> AddTags() 
        {
            var tags = new List<Tag>()
            {
                new() {Name = "Cờ", UrlSlug = "co",
                    Description = "Cờ"},
                new() {Name = "Mẹo", UrlSlug = "meo",
                    Description = "Các trò chơi mẹo"},
                new() {Name = "Nhóm", UrlSlug = "nhom",
                    Description = "Nhóm"},
                new() {Name = "Thể lực", UrlSlug = "the-luc",
                    Description = "Thể lực"},
                new() {Name = "Khó", UrlSlug = "kho",
                    Description = "Khó"},
                new() {Name = "Dễ", UrlSlug = "de",
                    Description = "Dễ"},
                new() {Name = "Kỹ năng", UrlSlug = "ky-nang",
                    Description = "Kỹ năng"}
            };

            _dbConText.AddRange(tags);
            _dbConText.SaveChanges();

            return tags;

        }

        private IList<Post> AddPosts(
            IList<Game> game,
            IList<Tag> tags) 
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "Hướng dẫn chơi trò Ô ăn quan",
                    ShortDescription = "Ô ăn quan là một trong những trò chơi dân gian phổ biến ở Việt Nam, " +
										"hãy cùng tìm hiểu cách chơi và những quy tắc trong trò chơi này.",
                    Description = "Ô ăn quan là trò chơi dân gian phổ biến của Việt Nam, được chơi trên một bàn cờ gồm 2 hàng lỗ đối diện nhau và nhiều hạt đậu, đá hoặc cát. Trò chơi có thể chơi với 2 người hoặc nhiều người cùng chơi.\r\n\r\nCách chơi:\r\n\r\n1. Bốc số để quyết định người chơi đầu tiên. Người chơi đầu tiên sẽ bắt đầu lượt đi đầu tiên.\r\n\r\n2. Người chơi lượt đi chọn một trong các lỗ trên bàn cờ và lấy hết số hạt đặt trong lỗ đó, rồi đặt hạt từng quả vào các lỗ tiếp theo theo chiều kim đồng hồ (mỗi lỗ chỉ được đặt một quả hạt).\r\n\r\n3. Nếu lượt đi của người chơi kết thúc ở một trong hai lỗ đối diện thì người chơi đó được tính điểm bằng số hạt trong lỗ đó.\r\n\r\n4. Nếu lượt đi kết thúc ở một lỗ trống hoặc lỗ có ít hơn hai hạt, thì lượt đi của người chơi đó kết thúc và chuyển lượt cho người chơi tiếp theo.\r\n\r\n5. Khi lượt đi của người chơi kết thúc ở một lỗ có nhiều hơn hai hạt, người chơi đó sẽ lấy hết số hạt trong lỗ đó và tiếp tục phân phát hạt theo chiều kim đồng hồ cho đến khi kết thúc ở một trong hai lỗ đối diện.r\n\r\n6. Khi có một lỗ đối diện với hai hoặc ba hạt, người chơi có thể ăn lỗ đó và lấy hết số hạt trong lỗ đối diện, rồi đặt vào túi của mình.\r\n\r\n7. Người chơi có quyền được ăn lỗ của đối thủ nếu lượt đi của mình kết thúc ở một lỗ có nhiều hơn hai hạt và lỗ đối diện của đối thủ có từ hai đến ba hạt. Người chơi đó sẽ lấy hết số hạt trong lỗ của đối thủ và đặt vào túi của mình.r\n\r\n8. Trò chơi kết thúc khi cả hai lỗ đối diện đều trống hoặc có ít hơn hai hạt. Người chơi có nhiều hạt nhất sẽ thắng cuộc.\r\n\r\nHy vọng các bạn có thể thưởng thức và tận hưởng trò chơi",
                    UrlSlug = "huong-dan-choi-tro-o-an-quan",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Game = game[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
				new()
				{
					Title = "Hướng dẫn chơi trò Cờ cá ngựa",
					ShortDescription = "Cờ cá ngựa là một trò chơi dân gian Việt Nam phổ biến, đơn giản và thú vị. " +
										"Hãy cùng tìm hiểu cách chơi trò chơi này qua hướng dẫn sau đây.",
					Description = "Cờ cá ngựa là trò chơi dân gian Việt Nam được yêu thích từ lâu với quy tắc đơn giản, phù hợp cho mọi lứa tuổi. " +
					"Trò chơi này được chơi trên một tấm bàn cờ có hình chữ nhật chia thành các ô vuông nhỏ. Mỗi người chơi sẽ điều khiển một đội ngựa của mình, " +
					"cố gắng hoàn thành việc đi hết đường đua trước khi đối thủ làm được điều đó." +
					"\r\n\r\nCách chơi:" +
					"\r\n\r\n1. Bàn cờ được chia thành 60 ô vuông nhỏ, có 6 hàng và 10 cột." +
					"\r\n2. Có 4 ngựa cho mỗi đội chơi." +
					"\r\n3. Mỗi người chơi sẽ chọn một đội ngựa để điều khiển." +
					"\r\n4. Người chơi sẽ tung xúc xắc để quyết định số lượng ô vuông mà ngựa của mình sẽ đi." +
					"\r\n5. Người chơi sẽ điều khiển ngựa của mình di chuyển trên đường đua." +
					"\r\n6. Nếu ngựa của bạn đến ô mà có một con ngựa khác của đối thủ đang đứng trên đó, ngựa của đối thủ sẽ bị trả lại về đầu đường đua và phải bắt đầu lại từ đầu." +
					"\r\n7. Người chơi đầu tiên hoàn thành việc đi hết đường đua sẽ chiến thắng." +
					"\r\n8. Đây là một trò chơi rất đơn giản và thú vị để giải trí cùng gia đình và bạn bè. Hãy thử chơi và tận hưởng niềm vui khi giành chiến thắng trong trò chơi này nhé!",
					UrlSlug = "huong-dan-choi-tro-co-ca-ngua",
					Published = true,
					PostedDate = new DateTime(2021, 10, 2, 14, 30, 0),
					ModifiedDate = null,
					ViewCount = 15,
					Game = game[1],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi Cờ tướng",
					ShortDescription = "Cờ tướng là trò chơi dân gian Việt Nam mang tính chiến thuật cao và được yêu thích.",
					Description = "Cờ tướng là một trò chơi cờ vua truyền thống của người Việt, được chơi trên một bàn cờ có kích thước 9x10 ô vuông và gồm 2 đội, mỗi đội có 16 quân cờ. " +
					"Dưới đây là hướng dẫn cơ bản để chơi cờ tướng:" +
					"\r\n\r\n1. Quân cờ và vị trí ban đầu: Có 16 quân cờ cho mỗi đội bao gồm: 1 tướng, 2 sĩ, 2 tượng, 2 mã, 2 xe, 2 pháo, và 5 tốt. Quân tướng được đặt ở giữa bàn cờ, hai bên sĩ đứng ngay cạnh tướng, hai bên tượng đứng ngay cạnh sĩ, hai bên mã đứng ngay cạnh tượng, hai bên xe đứng ở ngoài cùng, hai bên pháo đứng ở ngay sau hai bên mã, và năm quân tốt được đặt ở hàng thứ 3 và hàng thứ 6." +
					"\r\n\r\n2. Luật đi cờ: Mỗi lượt người chơi được phép di chuyển một quân cờ của mình đến một ô trống trên bàn cờ. Quân cờ tướng chỉ được đi trên các đường ngang và đường dọc trên bàn cờ, không được đi chéo. Một số quân cờ đặc biệt như tượng và sĩ chỉ được đi trên một nửa bàn cờ." +
					"\r\n\r\n3. Luật ăn cờ: Khi quân cờ của người chơi đến được một ô mà trên đó đang đứng một quân cờ của đối phương, người chơi có quyền ăn quân cờ của đối phương bằng cách đưa quân của mình đến ô đó và loại bỏ quân đối phương." +
					"\r\n\r\n4. Luật chiếu tướng: Nếu quân tướng của một người chơi bị đối thủ chiếu, người chơi đó phải di chuyển tướng ra khỏi tình trạng chiếu tướng trong lượt đi tiếp theo." +
					"\r\n\r\n5. Luật chặn tướng: Không được phép di chuyển quân cờ của mình đến ô nằm trực tiếp giữa hai quân tướng của đối phương." +
					"\r\n\r\n6. Thắng cuộc: Người chơi nào chiếu tướng hoặc hết quân trước sẽ thua cuộc." +
					"\r\n\r\nHy vọng hướng dẫn cơ bản trên sẽ giúp bạn bắt đầu chơi cờ tướng và tận hưởng niềm vui của trò chơi này.",
					UrlSlug = "huong-dan-choi-co-tuong",
					Published = true,
					PostedDate = new DateTime(2021, 9, 30, 10, 30, 0),
					ModifiedDate = null,
					ViewCount = 25,
					Game = game[2],
					Tags = new List<Tag>()
					{
						tags[0],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi Cờ vua",
					ShortDescription = "Cờ vua là trò chơi dân gian, truyền thống của nhiều quốc gia trên thế giới, trong đó có Việt Nam.",
					Description = "Cờ vua là một trò chơi đối kháng giữa hai người chơi trên một bàn cờ gồm 64 ô vuông, mỗi người chơi điều khiển các quân cờ của mình để tấn công và bảo vệ đế chế của mình. Dưới đây là hướng dẫn chi tiết cách chơi cờ vua:" +
					"\r\n\r\n1. Các quân cờ và cách di chuyển:\r\nTốt: di chuyển một ô thẳng tới phía trước, hoặc nếu chưa di chuyển, có thể đi hai ô thẳng tới phía trước.\r\nMã: di chuyển theo hình chữ L, đi 2 ô thẳng và 1 ô ngang hoặc ngược lại.\r\nTượng: di chuyển đường chéo một hoặc nhiều ô.\r\nXe: di chuyển đường thẳng một hoặc nhiều ô.\r\nHậu: di chuyển đường thẳng hoặc đường chéo một hoặc nhiều ô.\r\nVua: di chuyển một ô theo đường thẳng hoặc đường chéo." +
					"\r\n2. Luật chơi:\r\nMục tiêu của trò chơi là chiếu bí (đưa Vua đối phương vào thế bí) hoặc bắt Vua của đối phương.\r\nCác quân cờ chỉ di chuyển theo đường đi được cho phép, không được đi ngược chiều hoặc qua các quân cờ khác.\r\nNếu quân Vua bị chiếu thì người chơi phải giải quyết tình huống này, bằng cách di chuyển Vua, bắt quân tấn công hoặc đặt quân phòng ngự.\r\nKhông được tấn công Vua trái phép, ví dụ như đặt Vua vào thế chiếu hoặc tấn công Vua trực tiếp.\r\nKhi một quân cờ của một người chơi bị đặt ở vị trí của quân cờ đối phương, quân cờ đối phương bị bắt và phải được gỡ khỏi bàn cờ." +
					"\r\n3. Cách chiến thắng:\r\nNgười chơi chiến thắng khi bắt được quân Vua của đối phương hoặc khi đối phương không còn đường đi hợp lệ nào để di chuyển quân Vua." +
					"\r\nChơi cờ vua là một hoạt động giải trí thú vị và đòi hỏi sự tư duy, trí thông minh, chiến lược và kiên nhẫn. Chúc bạn có những giờ phút thư giãn và vui vẻ khi chơi cờ vua!",
					UrlSlug = "huong-dan-choi-co-vua",
					Published = true,
					PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 20,
					Game = game[3],
					Tags = new List<Tag>()
					{
						tags[0],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Đố vui",
					ShortDescription = "Đố vui là một trò chơi tinh thông qua câu hỏi và trả lời, phổ biến trong các buổi tiệc tùng, sinh hoạt, dã ngoại và giải trí với bạn bè và gia đình.",
					Description = "Đố vui là một trò chơi giải trí thông qua câu hỏi và trả lời, có thể được chơi đơn lẻ hoặc nhóm. " +
					"Trò chơi này yêu cầu người chơi đưa ra câu trả lời đúng cho một câu hỏi hoặc giải đố được đưa ra bởi người đố vui. " +
					"Câu hỏi và giải đố có thể liên quan đến nhiều lĩnh vực như lịch sử, văn học, khoa học, địa lý, nghệ thuật và nhiều lĩnh vực khác. " +
					"Để chiến thắng trò chơi, người chơi cần có kiến thức đa dạng và khả năng suy luận nhanh chóng.",
					UrlSlug = "huong-dan-choi-tro-do-vui",
					Published = true,
					PostedDate = new DateTime(2022, 4, 16, 8, 30, 0),
					ModifiedDate = null,
					ViewCount = 10,
					Game = game[4],
					Tags = new List<Tag>()
					{
						tags[1],
						tags[5]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Đá cầu",
					ShortDescription = "Bạn muốn tìm hiểu về trò chơi đá cầu truyền thống Việt Nam và cách chơi sao cho đúng kỹ thuật? Bài viết này sẽ giúp bạn hiểu rõ hơn về trò chơi này và cách thực hiện các động tác cơ bản",
					Description = "Đá cầu là một trò chơi truyền thống phổ biến ở Việt Nam, được chơi từ rất lâu đời và được yêu thích bởi sự đơn giản và thú vị của nó. Dưới đây là hướng dẫn chơi đá cầu:" +
					"\r\n\r\n1. Số người chơi: Đá cầu có thể chơi từ 2 đến 10 người." +
					"\r\n\r\n2. Cách chơi:\r\n\r\nNgười chơi đứng trong một vòng tròn và ném quả cầu lên để các người chơi khác đá bóng với tay hoặc chân để tránh quả cầu chạm đất.\r\nMỗi lần đá bóng thành công, người đó được tính điểm.\r\nTrong khi chơi, các người chơi có thể sử dụng cả hai tay và chân để đá bóng.\r\nNếu bóng chạm đất, người đó sẽ bị loại ra khỏi trò chơi.\r\nTrò chơi tiếp tục cho đến khi chỉ còn lại một người chơi cuối cùng." +
					"\r\n3. Điểm số: Người chơi tính điểm cho mỗi lần đá bóng thành công. Nếu bóng chạm đất, người đó sẽ không được tính điểm. Người có số điểm cao nhất sẽ thắng cuộc." +
					"\r\n\r\n4. Thông tin cần lưu ý:\r\n\r\nQuả cầu thường được làm bằng lông gà, đó là nguyên liệu truyền thống và tốt nhất để chơi trò chơi này.\r\nNgười chơi nên sử dụng cả tay và chân để đá bóng.\r\nNếu bạn chơi trên đất cỏ hoặc bãi cát, hãy chọn một khu vực phẳng để đá bóng.\r\nCó thể chơi trò chơi này bất cứ khi nào và ở bất kỳ địa điểm nào." +
					"\r\nĐá cầu là một trò chơi rất thú vị và đơn giản để chơi cùng bạn bè và gia đình. Hy vọng với hướng dẫn này, bạn có thể tận hưởng được trò chơi này.",
					UrlSlug = "huong-dan-choi-tro-da-cau",
					Published = true,
					PostedDate = new DateTime(2023, 2, 16, 8, 30, 0),
					ModifiedDate = null,
					ViewCount = 14,
					Game = game[5],
					Tags = new List<Tag>()
					{
						tags[3],
						tags[6]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Kéo co",
					ShortDescription = "Hãy tìm hiểu cách chơi trò chơi kéo co truyền thống của Việt Nam và tận hưởng những phút giây thú vị cùng bạn bè và gia đình.",
					Description = "Kéo co là một trò chơi truyền thống phổ biến ở Việt Nam, được chơi từ rất lâu đời và được yêu thích bởi tính thân thiện, đơn giản và thú vị của nó. Để chơi trò chơi này, người chơi sẽ chia thành hai đội, mỗi đội có số lượng người bằng nhau và cố gắng kéo dây thắt giữa hai bên để đưa đối phương vượt qua đường giới hạn.",
					UrlSlug = " huong-dan-choi-tro-keo-co",
					Published = true,
					PostedDate = new DateTime(2022, 3, 16, 8, 30, 0),
					ModifiedDate = null,
					ViewCount = 20,
					Game = game[6],
					Tags = new List<Tag>()
					{
						tags[3],
						tags[6]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Nhảy dây",
					ShortDescription = "Tìm hiểu cách chơi trò chơi nhảy dây truyền thống của Việt Nam và trải nghiệm niềm vui đơn giản nhưng không kém phần thú vị của nó.",
					Description = "Nhảy dây là một trò chơi truyền thống phổ biến ở Việt Nam, được chơi từ rất lâu đời và được yêu thích bởi sự đơn giản, thú vị và khỏe mạnh của nó. Để chơi trò chơi này, người chơi sẽ cần một sợi dây thun và ít nhất hai người giữ hai đầu dây thun và quay dây, trong khi người chơi cố gắng nhảy qua dây một cách liên tục và không chạm dây.",
					UrlSlug = " huong-dan-choi-tro-nhay-day",
					Published = true,
					PostedDate = new DateTime(2022, 3, 10, 8, 30, 0),
					ModifiedDate = null,
					ViewCount = 13,
					Game = game[7],
					Tags = new List<Tag>()
					{
						tags[3],
						tags[6]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Trọi gà",
					ShortDescription = "Tìm hiểu về trò chơi truyền thống đá gà của Việt Nam và hướng dẫn cách chơi để trải nghiệm niềm vui đơn giản của nó.",
					Description = "Trò đá gà là một trò chơi truyền thống phổ biến ở Việt Nam, được chơi từ rất lâu đời và được yêu thích bởi tính thách đấu, kịch tính và sự đam mê của nó. Để chơi trò chơi này, người chơi sẽ cần một đàn gà và một đấu trường để thi đấu.\r\n\r\nCác đội gà sẽ đối đầu nhau trong một trận đấu và sử dụng một loạt các chiến thuật để giành chiến thắng. Người chơi cần cẩn thận và khéo léo để không làm bị thương hoặc gây tổn thất cho gà. Sau khi đã giành chiến thắng, người chơi sẽ được hưởng niềm vui và thể hiện được tinh thần thách đấu.\r\n\r\nĐể chơi trò chơi đá gà, các bạn sẽ cần một đàn gà, một đấu trường và một số quan sát viên để đảm bảo tính công bằng và an toàn cho trận đấu.\r\n\r\nBước 1: Chuẩn bị sân chơi và các vật dụng cần thiết.\r\n\r\nBước 2: Chọn các gà chất lượng cao để thi đấu.\r\n\r\nBước 3: Xác định các đội gà và đặt cược trước khi trận đấu bắt đầu.\r\n\r\nBước 4: Khởi đầu trận đấu và sử dụng các chiến thuật để giành chiến thắng.\r\n\r\nBước 5: Tiếp tục thi đấu cho đến khi một đội giành chiến thắng. Người chơi cùng đội với đội giành chiến thắng sẽ được coi là người chiến thắng.\r\n\r\nTrong bài viết này, chúng ta đã hướng dẫn cách chơi trò đá gà truyền thống của Việt Nam và các thông tin cần lưu ý khi chơi trò chơi này.",
					UrlSlug = " huong-dan-choi-tro-troi-ga",
					Published = true,
					PostedDate = new DateTime(2022, 6, 12, 10, 15, 0),
					ModifiedDate = null,
					ViewCount = 15,
					Game = game[8],
					Tags = new List<Tag>()
					{
						tags[2],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Trọi cá",
					ShortDescription = "Tìm hiểu về trò chơi truyền thống đấu cá của Việt Nam",
					Description = "Trong trò chơi này, hai con cá sẽ đấu với nhau để xem con nào có thể chiến thắng.\r\n\r\nĐể chơi trò chơi trọi cá, người chơi sẽ cần hai con cá nhỏ, một chiếc cái lồng đặt xuống mặt đất và một cây gậy. Người chơi sẽ cho hai con cá vào lồng và đặt lồng xuống mặt đất. Sau đó, hai người chơi sẽ lần lượt dùng cây gậy đánh vào lồng để đẩy con cá của mình đi lùi hoặc đánh vào con cá của đối thủ để đẩy nó ra khỏi lồng.\r\n\r\nĐể chiến thắng, người chơi cần phải đẩy con cá của đối thủ ra khỏi lồng hoặc làm cho con cá đó nhảy ra khỏi lồng. Trò chơi sẽ kết thúc khi một con cá được đẩy ra khỏi lồng hoặc khi hết thời gian quy định.\r\n\r\nBước 1: Chuẩn bị hai con cá nhỏ, một chiếc cái lồng và một cây gậy.\r\n\r\nBước 2: Đặt hai con cá vào lồng và đặt lồng xuống mặt đất.\r\n\r\nBước 3: Hai người chơi lần lượt dùng cây gậy đánh vào lồng để đẩy con cá của mình đi lùi hoặc đánh vào con cá của đối thủ để đẩy nó ra khỏi lồng.\r\n\r\nBước 4: Chiến thắng khi đẩy con cá của đối thủ ra khỏi lồng hoặc làm cho con cá đó nhảy ra khỏi lồng.\r\n\r\nTrong bài viết này, chúng ta đã hướng dẫn cách chơi trò chơi cá đá nhau truyền thống của Việt Nam và các thông tin cần lưu ý khi chơi trò chơi này.",
					UrlSlug = " huong-dan-choi-tro-troi-ca",
					PostedDate = new DateTime(2022, 7, 20, 14, 0, 0),
					ModifiedDate = null,
					ViewCount = 9,
					Game = game[10],
					Tags = new List<Tag>()
					{
						tags[5],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Trọi kiến",
					ShortDescription = "Tìm hiểu về trò chơi truyền thống đấu kiến của Việt Nam và hướng dẫn cách chơi để trải nghiệm niềm vui đơn giản của nó.",
					Description = "Trò đấu kiến là một trò chơi truyền thống phổ biến ở Việt Nam, được chơi từ rất lâu đời và được yêu thích bởi tính thách đấu, kịch tính và sự đam mê của nó. Để chơi trò chơi này, người chơi sẽ cần hai con kiến và một sân đấu để thi đấu.\r\n\r\nCác kiến sẽ đối đầu nhau trong một trận đấu và sử dụng một loạt các chiến thuật để giành chiến thắng. Người chơi cần cẩn thận và khéo léo để không làm bị thương hoặc gây tổn thất cho kiến. Sau khi đã giành chiến thắng, người chơi sẽ được hưởng niềm vui và thể hiện được tinh thần thách đấu.\r\n\r\nĐể chơi trò chơi đấu kiến, các bạn sẽ cần hai con kiến, một sân đấu và một số quan sát viên để đảm bảo tính công bằng và an toàn cho trận đấu.\r\n\r\nBước 1: Chuẩn bị sân đấu và các vật dụng cần thiết.\r\n\r\nBước 2: Chọn hai con kiến đủ sức khỏe để thi đấu.\r\n\r\nBước 3: Xác định các đội kiến và đặt cược trước khi trận đấu bắt đầu.\r\n\r\nBước 4: Khởi đầu trận đấu và sử dụng các chiến thuật để giành chiến thắng.\r\n\r\nBước 5: Tiếp tục thi đấu cho đến khi một con kiến giành chiến thắng. Người chơi cùng đội với con kiến giành chiến thắng sẽ được coi là người chiến thắng.\r\n\r\nTrong bài viết này, chúng ta đã hướng dẫn cách chơi trò đấu kiến truyền thống của Việt Nam và các thông tin cần lưu ý khi chơi trò chơi này.",
					UrlSlug = " huong-dan-choi-tro-troi-kien",
					Published = true,
					PostedDate = new DateTime(2022, 7, 20, 14, 0, 0),
					ModifiedDate = null,
					ViewCount = 9,
					Game = game[10],
					Tags = new List<Tag>()
					{
						tags[5],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Trốn tìm",
					ShortDescription = "Trốn tìm là một trò chơi truyền thống phổ biến ở Việt Nam",
					Description = "Trong trò chơi này, một người sẽ được chọn làm \"người tìm\" và các người chơi còn lại sẽ là \"người trốn\". Người tìm sẽ đếm đến một số nhất định, trong khi các người trốn sẽ tìm nơi che giấu để tránh bị tìm thấy. Sau đó, người tìm sẽ bắt đầu tìm kiếm các người trốn. Người trốn cuối cùng không bị tìm thấy sẽ thắng trò chơi.\r\n\r\nĐể chơi trò chơi Trốn tìm, bạn sẽ cần một nhóm bạn và một không gian đủ rộng để tránh sự va chạm khi chơi. Các người chơi sẽ cần thỏa thuận về thời gian và nơi để bắt đầu trò chơi. Nếu bạn muốn tăng thêm sự thú vị cho trò chơi, bạn có thể thiết kế các vật phẩm che giấu để người trốn sử dụng, hoặc quy định các quy tắc nhất định để tăng thêm sự khó khăn cho người tìm.\r\n\r\nBước 1: Xác định số lượng người chơi và người tìm trong trò chơi.\r\n\r\nBước 2: Thống nhất về không gian và thời gian để bắt đầu trò chơi.\r\n\r\nBước 3: Người tìm sẽ đếm đến một số nhất định, trong khi các người chơi khác sẽ tìm nơi che giấu.\r\n\r\nBước 4: Người tìm bắt đầu tìm kiếm các người trốn và cố gắng tìm thấy tất cả các người trốn.\r\n\r\nBước 5: Người trốn cuối cùng không bị tìm thấy sẽ thắng trò chơi.\r\n\r\nTrò chơi Trốn tìm là một trò chơi vui nhộn và thú vị, phù hợp cho mọi lứa tuổi và được yêu thích",
					UrlSlug = " huong-dan-choi-tro-tron-tim",
					Published = true,
					PostedDate = new DateTime(2021, 6, 20, 14, 0, 0),
					ModifiedDate = null,
					ViewCount = 35,
					Game = game[11],
					Tags = new List<Tag>()
					{
						tags[2],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Rồng rắn lên mây",
					ShortDescription = "Tìm hiểu về trò chơi rồng rắn lên mây của Việt Nam.",
					Description = "Chuẩn bị trước khi chơi\r\nNgười chơi: Trò chơi dân gian rồng rắn lên mây không giới hạn số người tham gia. Tuy nhiên, số lượng người phù hợp nhất là khoảng 6 – 8 người để trò chơi được thú vị nhất và thoải mái chạy nhảy mà không bị xô đẩy nhiều. Đặc biệt, rồng rắn lên mây cần có một thành viên đứng ra làm người quản trò.\r\nĐịa điểm tổ chức: Để trò chơi thêm phần hấp dẫn, bạn nên lựa chọn địa điểm chơi có diện tích rộng và bằng phẳng để người chơi thoải mái mà không gặp phải cản trở, nguy hiểm khi chơi. Ví dụ: Sân trường, sân chơi tập thể, sân bóng…\r\nCách chơi rồng rắn lên mây\r\nTất cả các thành viên tham gia sẽ oẳn tù tì, người thua cuộc sẽ đóng vai trò là thầy thuốc. Những thành viên còn lại sẽ làm “rồng rắn”, chọn ra một người đi đầu bằng cách oẳn tù tì. Thông thường, người đứng đầu sẽ là thành viên to lớn, khỏe và nhanh nhẹn nhất. Các thành viên còn lại sẽ túm đuôi áo nhau lần lượt hoặc tay ôm lưng nhau.\r\n\r\nBắt đầu trò chơi\r\nThầy thuốc đứng cố định tại một vị trí (hay còn gọi là nhà thầy thuốc).\r\n\r\nĐoàn rồng rắn bám đuôi nhau đi theo người đứng đầu, đi lượn vòng vèo, vừa đi vừa đọc đồng dao:\r\n\r\nRồng rắn lên mây\r\nCó cây núc nắc\r\nCó nhà hiển binh\r\nThầy thuốc có nhà hay không?\r\n\r\nKhi hát đến chữ “không” cuối cùng thì cũng là lúc đầu của đoàn rồng rắn đứng ngay trước mặt thầy thuốc. Cả đoàn rồng rắn dừng lại, chăm chú nghe thầy thuốc trả lời.\r\n\r\nKhông. Thầy thuốc vắng nhà (đi chơi, đi chợ…)\r\n\r\nKhi đó, đoàn rồng rắn lại tiếp tục vừa đi vừa hát cho đến khi thầy thuốc trả lời là “có”.\r\n\r\nTừ đó, thầy thuốc và đoàn rồng rắn cùng nhau đối đáp:\r\n\r\nThầy thuốc: Có, mẹ con rồng rắn đi đâu?\r\nRồng rắn: Rồng rắn đi lấy thuốc cho con\r\nThầy thuốc: Con lên mấy?\r\nRồng rắn: Con lên một\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên hai\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên ba\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên bốn\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên năm\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên sáu\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên bảy\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên tám\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên chín\r\nThầy thuốc: Thuốc chẳng ngon\r\nRồng rắn: Con lên mười\r\nThầy thuốc: Thuốc ngon vậy, xin khúc đầu\r\nRồng rắn: Cùng xương cùng xẩu.\r\nThầy thuốc: Xin khúc giữa.\r\nRồng rắn: Cùng máu cùng me.\r\nThầy thuốc: Xin khúc đuôi\r\nRồng rắn: Tha hồ mà đuổi.\r\nKhi đối thoại với thầy thuốc, đoàn rồng rắn có thể không nhất thiết phải trả lời tuần tự từ 1 đến 10 mà có thể trả lời ngắt quãng tuổi. Ví dụ: 1 – 5 – 7… để rút ngắn thời gian đối thoại.\r\n\r\nKhi đọc đến chữ “tha hồ mà đuổi”, thầy thuốc đuổi bắt đoàn rồng rắn, người đứng đầu sẽ dang tay cản thầy thuốc, thầy thuốc tìm mọi cách để bắt được “khúc đuôi” (trẻ đứng cuối). Nếu thầy thuốc bắt được khúc đuôi thì bạn đứng khúc đuôi sẽ bị loại khỏi cuộc chơi. Nếu rồng rắn bị đứt khúc (nhiều bạn nhỏ bị rời khỏi đoàn) hoặc bị ngã thì cũng bị thua, loại khỏi cuộc chơi.\r\n\r\nKhi có người bị loại, trò chơi sẽ bắt đầu lại nhưng không bao gồm các bạn bị loại. Trò chơi sẽ tiếp tục diễn ra cho đến khi đoàn rồng rắn ngắn dần hoặc còn 1, 2 bạn chơi.",
					UrlSlug = " huong-dan-choi-tro-rong-ran-len-may",
					Published = true,
					PostedDate = new DateTime(2021, 3, 20, 14, 0, 0),
					ModifiedDate = null,
					ViewCount = 30,
					Game = game[12],
					Tags = new List<Tag>()
					{
						tags[2],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Bầu cua",
					ShortDescription = "Bầu cua là một trò chơi truyền thống của Việt Nam, thường được chơi trong các dịp lễ tết. Hãy cùng tìm hiểu cách chơi bầu cua để có thể tham gia các trò chơi cùng gia đình và bạn bè nhé!",
					Description = "Bầu cua là trò chơi đơn giản nhưng lại rất thú vị. Người chơi sẽ đặt cược vào một trong sáu con vật: tôm, cá, gà, bầu, nai, hột vịt. Sau đó, người chơi sẽ tung ba viên xúc xắc, mỗi viên có một hình ảnh tương ứng với sáu con vật. Nếu cả ba viên xúc xắc đều hiện hình ảnh của con vật mà người chơi đã đặt cược, thì người đó sẽ nhận được số tiền đặt cược nhân với tỷ lệ cược.\r\n\r\nNếu chỉ có một hoặc hai viên xúc xắc hiện hình ảnh của con vật đó, thì người chơi sẽ không được nhận gì cả. Nếu cả ba viên xúc xắc đều không hiện hình ảnh của con vật đó, thì người chơi sẽ mất số tiền đặt cược.\r\n\r\nCách đặt cược trong bầu cua rất đơn giản. Người chơi chỉ cần đặt số tiền vào bất kỳ con vật nào mà mình muốn cược. Tuy nhiên, để có thể chơi thắng, người chơi nên chú ý đến tỷ lệ cược của mỗi con vật. Tỷ lệ cược sẽ thay đổi tùy theo từng địa phương và từng người chơi khác nhau.\r\n\r\nNếu bạn muốn tăng cơ hội thắng của mình trong trò chơi bầu cua, hãy chú ý quan sát xem con vật nào xuất hiện nhiều nhất trong các lượt tung xúc xắc trước đó. Tuy nhiên, đây chỉ là một chiêu thức đơn giản và không đảm bảo sẽ giúp bạn thắng trong mọi trường hợp.",
					UrlSlug = " huong-dan-choi-tro-bau-cua",
					Published = true,
					PostedDate = new DateTime(2022, 5, 1, 9, 0, 0),
					ModifiedDate = null,
					ViewCount = 25,
					Game = game[13],
					Tags = new List<Tag>()
					{
						tags[4],
						tags[5]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò Oẳn tù xì",
					ShortDescription = "Oẳn tù xì là một trò chơi bài truyền thống của Việt Nam. Trò chơi này đòi hỏi sự tinh ý và sự thông minh của người chơi. Hãy cùng tìm hiểu cách chơi oẳn tù xì để có thể tham gia các trò chơi cùng gia đình và bạn bè nhé!",
					Description = "1. Thể hiện của trò chơi\r\nTrước khi bắt đầu trò chơi, các cô cần phổ biến cho cháu các thể hiện trong trò chơi. Dưới đây là ba thể hiện phổ biến nhất:\r\n\r\nCái Búa: Nắm các ngón tay lại\r\nCái Kéo: Hai ngón tay (ngón trỏ, ngón giữa) mở ra như chữ V, các ngón còn lại sẽ được nắm lại.\r\nCái Bao: Xòe cả bàn tay ra.\r\nThể hiện trong trò chơi Oẳn tù tì\r\n\r\nNgoài ra, một số nơi sẽ có thêm, tùy từng địa phương, tùy từng giao kèo.\r\n\r\nCái Đục: Ngón trỏ sẽ duỗi ra, các ngón còn lại sẽ được nắm lại.\r\nCái Giếng: Được thể hiện bằng vòng tròn tạo ra bởi ngón cái và ngón trỏ.\r\n2. Luật chơi\r\nLuật chơi Oẳn tù tì\r\n\r\nĐể phân định thắng thua, người chơi cần nằm lòng quy tắc:\r\n\r\nBúa ăn kéo, thua Bao.\r\nKéo cắt được Bao, thua Búa.\r\nBao chùm được Búa, bị cắt bởi Kéo.\r\nNếu người chơi ra dấu giống nhau sẽ được coi là hòa và sẽ tiến hành lại.\r\nTrong trường hợp có thêm đục và giếng cần nhớ:\r\n\r\nĐục ăn kéo, ăn bao, thua búa, thua giếng.\r\nGiếng ăn búa, ăn đục, ăn kéo, thua bao.\r\n3. Cách chơi\r\nTrò chơi có thể bắt đầu khi số lượng người chơi từ 2 trở lên, cùng đứng hoặc cùng ngồi. Tuy nhiên, trong trường hợp đông quá phải chia thành các nhóm cho dễ quan sát cũng như phân định thắng thua.\r\n\r\nNgười chơi mặt sẽ hướng vào nhau, tay phải nắm, đung đưa theo nhịp câu hát:\r\n\r\n\"Oẳn tù tì\r\nRa cái gì?\r\nRa cái này!\"\r\n\r\nCâu hát vừa dứt, người chơi sẽ nhanh chóng xòe tay theo các hình kéo, búa, bao. Tất cả phải đưa ra cùng một lúc, không được người ra trước người ra sau.\r\nĐể phân biệt thắng thua sẽ tuân theo luật chơi ở trên.\r\n\r\nĐể tăng sức cuốn hút cho trò chơi, có thể đưa ra một số hình phạt ví dụ như người thua sẽ bị người thắng búng nhẹ vào tai hay búng vào tay, …\r\n\r\nTrên đây là hướng dẫn cách chơi trò chơi Oẳn tù tì cho trẻ mẫu giáo giúp các bé rèn luyện tính phản xạ có thể áp dụng trong nhiều trò chơi khác nhau. Cảm ơn các bạn đã theo dõi.",
					UrlSlug = " huong-dan-choi-tro-oan-tu-xi",
					Published = true,
					PostedDate = new DateTime(2022, 3, 20, 14, 0, 0),
					ModifiedDate = null,
					ViewCount = 40,
					Game = game[14],
					Tags = new List<Tag>()
					{
						tags[1],
						tags[4]
					}
				},
				new()
				{
					Title = "Hướng dẫn chơi trò chắn",
					ShortDescription = "Chắn là một trong những hình thức chơi bài rất được người Việt Nam ưa chuộng và yêu thích.",
					Description = "1. Tìm hiểu bài chắn\r\nTrên cơ sở nguồn gốc bài Tổ tôm, người ta sáng tạo ra hình thức chơi bài chắn, bao gồm hai phiên bản khác nhau được phân theo số lượng người tham gia. Trong đó, loại chắn thứ nhất gồm 4 người chơi (được gọi là chắn bí tứ), đây đồng thời cũng là loại chắn thông dụng nhất. Loại chắn thứ hai gồm 5 người chơi (được gọi là chắn bí ngũ).\r\n\r\nTìm hiểu bài chắn\r\nTìm hiểu bài chắn\r\nKhác với bài Tổ tôm, người ta sử dụng hết 120 quân bài thì chỉ có khoảng 100 quân được sử dụng trong bài chắn, 20 quân còn lại được lược bớt bao gồm: Nhất sách, nhất vạn, nhất văn, lão và thang. Trong chắn, quân bài được nhận biết bằng hình ảnh và chữ, nhớ bài bằng cách nhìn vào hình ảnh tượng hình ở mặt quân bài hoặc dựa vào chữ ở đầu mỗi quân bài. Nhị, Tam, Tứ, Ngũ, Lục, Thất, Bát, Chi là những chữ nằm bên phía tay phải. Vạn, Văn, Sách là những chữ nằm bên tay trái.\r\n\r\nMẹo để nhớ các ký tự trong bài chắn thường được truyền miệng qua câu nói ngắn gọn như sau: “vạn vuông, văn chéo, sách loằng ngoằng”. Như vậy, những quân vạn thường có ký tự vuông, văn thì ký tự thường hình chéo và sách thì có ký tự hơi lằng ngoằng.2. Hướng dẫn chơi chắn cho người mới\r\nMặc dù khá đơn giản với nhiều người, nhưng với những người mới chơi, cách chơi chắn phải được học một cách bài bản. Hướng dẫn chơi chắn sau đây sẽ giúp bạn hiểu rõ hơn về luật chơi, cách đánh chắn cũng như cách tính các lỗi phạt cơ bản trong chắn.\r\n\r\nHướng dẫn chơi chắn cho người mới\r\nHướng dẫn chơi chắn cho người mới\r\n2.1. Số lượng tham gia trong bài chắn\r\nVề cơ bản như đã giới thiệu ngay từ đầu, đánh chắn có hai hình thức chơi, dựa vào số lượng người chơi tham gia vào ván đầu. Loại chắn bí tứ được ưa chuộng nhất, do đó số lượng người chơi trong bài chắn thông thường sẽ bao gồm 4 người. Trong đó, tổng số bài sẽ được chia cho mỗi người, mỗi người được chia 19 lá, số lượng lá bài còn lại khi đã chia được đặt vào trung tâm (chính giữa) ván đấu, hay còn gọi là Nọc.\r\n\r\n2.2. Cách chia bài chắn\r\nCụ thể, trong bài chắn, các quân bài sẽ được chia thành 5 phần, sau khi chia sẽ dư lại khoảng 5 quân bài. Người chơi lấy 5 quân lẻ này kết hợp với một phần bài bất kỳ để cấu thành Nọc. Kết hợp bài có thể tùy ý hoặc người thắng cuộc ở ván trước là người gộp. Tiếp đó, 1 quân trong chồng bài Nọc sẽ được rút ngẫu nhiên, lật quân bài lên vào một phần bài bất kỳ trong 4 phần bài còn lại và cấu thành một phần bài cái.\r\n\r\nCách chia bài chắn\r\nCách chia bài chắn\r\nSau đó, người chơi phải tiến hành bốc cái thì mới xác định được ai là người đánh đầu tiên và ai được phần bài nào. Cụ thể, có bốn người chơi tương ứng với các vị trí 1, 2, 3 và 4, sắp xếp sao lần lượt từ trái qua phải, đảm bảo người chơi số 2 và người chơi số 4 ngồi chéo nhau. Người chơi số 2 bốc cái được quân thất vạn, đếm từ B sẽ là 1, đến D sẽ là 7. Như vậy, phần bài cái thuộc về D. Từng người còn lại sẽ được chia những phần bài xung quanh phần bài cái. Phía phải bài cái được chia cho người chơi số 1 (nghĩa là người chơi có vị trí bên tay phải người được phần bài cái là người chơi số 4), phần tiếp nữa đưa cho người chơi số 2, còn phần bên trái bài cái được chia cho người chơi số 3.\r\n\r\nTương tự như các hình thức bài khác, xếp bài trong chắn phải cấu thành nên các dạng bài bao gồm: Chắn, ba đầu, cạ, què. Trong đó, hai quân bài giống hệt nhau được gọi là chắn, 3 quân bài khác chất cùng số là ba đầu, 2 quân bài khác chất cùng số là cạ, và những quân bài lẻ không thể kết hợp được gọi là què.2.3. Cách đánh chắn chi tiết\r\nCách đánh chắn chi tiết\r\nCách đánh chắn chi tiết\r\nHướng dẫn chơi chắn chỉ ra rằng, trong quá trình chơi, người chơi có thể thực hiện cách đánh chắn bằng những hành động như sau:\r\n\r\n- Cửa chì: Tính từ trái qua phải, đây là cửa của người chơi được ưu tiên ăn.\r\n\r\n- Bốc Nọc: Hành động bốc một quân bài trong Nọc, lật ngửa bài vào cửa chì.\r\n\r\n- Ăn: Hành động kết hợp hai quân dưới và trên để tạo thành Cà hoặc Chắn.\r\n\r\n- Chíu: Hành động ăn quân dưới chiếu nếu có 3 quân bài giống nhau, trong khi cũng có một quân tương tự ở dưới chiếu.\r\n\r\n- Ù: Khi tất cả quân bài của người chơi (bao gồm 19 quân), kể cả những quân bài ăn được kết hợp với một quân được rút ra từ Nọc (bất kể người chơi nào rút) để tạo thành 10 bộ Cạ hoặc Chắn. Trong đó, phải đảm bảo có tối thiểu 6 Chắn.",
					UrlSlug = " huong-dan-choi-tro-chan",
					Published = true,
					PostedDate = new DateTime(2022, 4, 20, 14, 0, 0),
					ModifiedDate = null,
					ViewCount = 20,
					Game = game[15],
					Tags = new List<Tag>()
					{
						tags[1],
						tags[5]
					}
				}
			};

            _dbConText.AddRange(posts); 
            _dbConText.SaveChanges();
        
            return posts;
        }
        
    }
}
