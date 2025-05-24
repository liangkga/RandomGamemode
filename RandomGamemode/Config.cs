using System.ComponentModel;
using Exiled.API.Interfaces;

namespace RandomGamemode
{
	public sealed class Config : IConfig
	{
		[Description( "插件是否启用。" )]
		public bool IsEnabled { get; set; } = true;

		[Description( "是否在控制台中显示调试消息。" )]
		public bool Debug { get; set; } = false;


		[Description( "每回合开始时激活游戏模式的几率。必须是整数。" )]
		public int GamemodeChance { get; set; } = 10;

		[Description( "游戏模式回合开始时显示的文本。使用 {0} 作为游戏模式的名称。" )]
		public string StartText { get; set; } = "<color=red>{0} 回合已开始！</color>";

		[Description( "游戏模式回合结束时显示的文本。使用 {0} 作为游戏模式的名称。" )]
		public string EndText { get; set; } = "<color=red>{0} 回合已结束。</color>";


		[Description( "启用/禁用躲避球游戏模式。" )]
		public bool DodgeBallEnabled { get; set; } = true;

		[Description( "躲避球回合开始时出现的说明。应少于250个字符。" )]
		public string DodgeBallText { get; set; } = "通过向玩家投掷躲避球来消灭他们以获胜！";


		[Description( "启用/禁用花生突袭游戏模式。" )]
		public bool PeanutRaidEnabled { get; set; } = true;

		[Description( "花生突袭回合开始时出现的说明。应少于250个字符。" )]
		public string PeanutRaidText { get; set; } = "花生必须在D级人员逃脱并成为混沌分裂者之前阻止他们！";


		[Description( "启用/禁用蓝屏死机游戏模式。" )]
		public bool BlueScreenOfDeathEnabled { get; set; } = true;

		[Description( "蓝屏死机回合开始时出现的说明。应少于250个字符。" )]
		public string BlueScreenOfDeathText { get; set; } = "科学家有15分钟的时间禁用SCP-079！";


		[Description( "启用/禁用活死人书呆子之夜游戏模式。" )]
		public bool LivingNerdEnabled { get; set; } = true;

		[Description( "活死人书呆子之夜回合开始时出现的说明。应少于250个字符。" )]
		public string LivingNerdText { get; set; } = "D级人员必须抵御杀手科学家的攻击！";


		[Description( "启用/禁用随机化器游戏模式。" )]
		public bool RandomizerEnabled { get; set; } = true;

		[Description( "随机化器回合开始时出现的说明。应少于250个字符。" )]
		public string RandomizerText { get; set; } = "每个人都被分配了随机的角色、物品和出生点。友军伤害已启用。最后存活的玩家获胜！";


		[Description( "启用/禁用烦人的拟态游戏模式。" )]
		public bool AnnoyingMimicryEnabled { get; set; } = true;

		[Description( "烦人的拟态回合开始时出现的说明。应少于250个字符。" )]
		public string AnnoyingMimicryText { get; set; } = "SCP-939可以通过杀死D级人员进行复制。D级人员必须反击。禁止逃往地表。";


		[Description( "启用/禁用禁闭游戏模式。" )]
		public bool LockedInEnabled { get; set; } = true;

		[Description( "禁闭回合开始时出现的说明。应少于250个字符。" )]
		public string LockedInText { get; set; } = "禁止逃往地表和轻度收容区封锁。MTF将在入口区域生成，混沌分裂者将在重度收容区生成。";


		[Description( "启用/禁用感染游戏模式。" )]
		public bool InfectionEnabled { get; set; } = true;

		[Description( "感染回合开始时出现的说明。应少于250个字符。" )]
		public string InfectionText { get; set; } = "被SCP-049杀死的玩家会自动复活为更强大的SCP-049-2。";


		[Description( "启用/禁用像拉里一样生活游戏模式。" )]
		public bool LivingLikeLarryEnabled { get; set; } = true;

		[Description( "像Larry一样生活回合开始时出现的说明。应少于250个字符。" )]
		public string LivingLikeLarryText { get; set; } = "D级人员必须逃离SCP-106的追捕并成为混沌分裂者！";
	}
}
