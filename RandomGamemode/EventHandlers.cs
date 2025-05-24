using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomGamemode
{
	enum的游戏模式
	{
		无效,
		躲避球,
		花生突袭,
		蓝屏死机,
		活死人书呆子之夜,
		随机化器,
		烦人的拟态,
		禁闭,
		感染,
		像拉里一样生活
	}

	public class EventHandlers
	{
		private Plugin plugin;
		private Gamemode CurrentGamemode;
		private bool FriendlyFireDefault;
		System.Random rand = new System.Random();

		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		private void StartGamemode( Gamemode gm )
		{
			CurrentGamemode = gm;
			switch ( gm )
			{
				case Gamemode.Dodgeball: Timing.RunCoroutine( DodgeBall() ); break;
				case Gamemode.PeanutRaid: Timing.RunCoroutine( PeanutRaid() ); break;
				case Gamemode.BlueScreenOfDeath: Timing.RunCoroutine( BlueScreenOfDeath() ); break;
				case Gamemode.NightOfTheLivingNerd: Timing.RunCoroutine( NightOfTheLivingNerd() ); break;
				case Gamemode.Randomizer: Timing.RunCoroutine( Randomizer() ); break;
				case Gamemode.AnnoyingMimicry: Timing.RunCoroutine( AnnoyingMimicry() ); break;
				case Gamemode.LockedIn: Timing.RunCoroutine( LockedIn() ); break;
				case Gamemode.Infection: Timing.RunCoroutine( Infection() ); break;
				case Gamemode.LivingLikeLarry: Timing.RunCoroutine( LivingLikeLarry() ); break;
			}
			Map.Broadcast( 6, string.Format( plugin.Config.StartText, Plugin.GetGamemodeName( CurrentGamemode ) ) );
		}

		private IEnumerator<float> DelayedRoundEnd( float delay )
		{
			yield return Timing.WaitForSeconds( delay );
			Round.EndRound( true );
		}

		#region Gamemodes
		public IEnumerator<float> DodgeBall()
		{
			FriendlyFireDefault = ServerConsole.FriendlyFire;
			ServerConsole.FriendlyFire = true;
			yield return Timing.WaitForSeconds( 3f );

			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.FacilityGuard );
				}

				yield return Timing.WaitForSeconds( 0.5f );
				ply.ClearInventory();

				for ( int i = 0; i < 7; i++ )
				{
					ply.AddItem( ItemType.SCP018 );
				}

				ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.NtfCaptain ).Position;
			}
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.DodgeBallText );
		}

		public IEnumerator<float> PeanutRaid()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.ClassD );
					yield return Timing.WaitForSeconds( 0.5f );
					ply.Scale *= 0.25f;
				}
				else
				{
					ply.Role.Set( RoleTypeId.Scp173 );
				}
			}
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.PeanutRaidText );
		}

		public IEnumerator<float> BlueScreenOfDeath()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.Scp079 );
					( ply.Role as Scp079Role ).Level = 3;
				}
				else
				{
					ply.Role.Set( RoleTypeId.Scientist );
					ply.ClearInventory();
				}
			}
			Map.ChangeLightsColor( Color.blue );
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.BlueScreenOfDeathText );
		}

		public IEnumerator<float> NightOfTheLivingNerd()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.Scientist );
					ply.ClearInventory();
					ply.AddItem( ItemType.GunLogicer );
					ply.AddItem( ItemType.Flashlight );
					ply.AddItem( ItemType.KeycardFacilityManager );
					ply.SetAmmo( AmmoType.Nato762, 1000 );
					ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.Scp939 ).Position;
					ply.EnableEffect( EffectType.MovementBoost );
				}
				else
				{
					ply.Role.Set( RoleTypeId.ClassD );
					ply.AddItem( ItemType.Flashlight );
					ply.AddItem( ItemType.SCP268 );
				}
			}
			Map.TurnOffAllLights( 5000 );
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.LivingNerdText );
		}

		public IEnumerator<float> Randomizer()
		{
			List<Player> PlyList = new List<Player>();
			FriendlyFireDefault = ServerConsole.FriendlyFire;
			ServerConsole.FriendlyFire = true;
			yield return Timing.WaitForSeconds( 3f );

			RoleTypeId[] roles = new RoleTypeId[] {
				RoleTypeId.NtfCaptain, RoleTypeId.ChaosConscript, RoleTypeId.ClassD,
				RoleTypeId.FacilityGuard, RoleTypeId.Scientist
			};

			RoleTypeId[] scps = new RoleTypeId[] {
				RoleTypeId.Scp049, RoleTypeId.Scp0492, RoleTypeId.Scp096,
				RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939
			};

			// 设置随机SCP
			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			yield return Timing.WaitForSeconds( 1f );

			Player scp = PlyList.RandomItem();
			scp.Role.Set( scps.RandomItem() );
			PlyList.RemoveAt( PlyList.IndexOf( scp ) );

			// 为其余玩家设置随机角色
			foreach ( Player ply in PlyList )
			{
				ply.Role.Set( roles.RandomItem() );
			}

			yield return Timing.WaitForSeconds( 1f );

			// 设置随机出生点
			foreach ( Player ply in Player.List )
			{
				if ( ply.Role == RoleTypeId.Scp0492 )
					ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.ClassD ).Position;
				else
					ply.Position = RoleExtensions.GetRandomSpawnLocation( roles.RandomItem() ).Position;
			}

			// 设置随机物品栏物品
			Array items = Enum.GetValues( typeof( ItemType ) );
			foreach ( Player ply in Player.List )
			{
				ply.ClearInventory();
				ply.AddItem( ItemType.KeycardO5 );
				for ( int i = 0; i < 7; i++ )
				{
					ply.AddItem( ( ItemType ) items.GetValue( rand.Next( items.Length ) ) );
				}
			}
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.RandomizerText );
		}

		public IEnumerator<float> AnnoyingMimicry()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
                    ply.Role.Set( RoleTypeId.Scp939 );
                }
				else
				{
                    ply.Role.Set( RoleTypeId.ClassD );
					ply.AddItem( ItemType.Jailbird );
                }
			}
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.AnnoyingMimicryText );
		}

		public IEnumerator<float> LockedIn()
		{
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.LockedInText );
		}

		public IEnumerator<float> Infection()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
					ply.Role.Set( RoleTypeId.Scp049 );
			}
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.InfectionText );
		}

		public IEnumerator<float> LivingLikeLarry()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.ClassD );
					ply.Position = Door.Get( DoorType.Scp173Bottom ).Position + Vector3.up * 2;
					ply.AddItem( ItemType.KeycardO5 );
				}
				else
				{
					ply.Role.Set( RoleTypeId.Scp106 );
					ply.Position = Door.Get( DoorType.Scp173Gate ).Position + Vector3.up * 2;
				}
			}
			yield return Timing.WaitForSeconds( 5f );
			Map.Broadcast( 10, plugin.Config.LivingLikeLarryText );
		}
		#endregion

		#region Events
		public void OnRoundStart()
		{
			if ( Plugin.NextGamemode != Gamemode.Invalid )
			{
				StartGamemode( Plugin.NextGamemode );
				Plugin.NextGamemode = Gamemode.Invalid;
			}
			else if ( rand.Next( 1, 101 ) <= plugin.Config.GamemodeChance )
			{
				CurrentGamemode = Plugin.EnabledList.RandomItem();
				StartGamemode( CurrentGamemode );
			}
			if ( CurrentGamemode == Gamemode.BlueScreenOfDeath )
			{
				Timing.RunCoroutine( DelayedRoundEnd( 900 ), "DelayedRoundEnd" );
			}
		}
		
		public void OnRoundEnding( EndingRoundEventArgs ev )
		{
			// 防止在所有人都属于同一队伍时随机化器回合结束
			int totalalive = 0;
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsAlive )
					totalalive++;
			}

			if ( CurrentGamemode == Gamemode.Randomizer && totalalive > 1 )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnRoundEnd( RoundEndedEventArgs ev )
		{
			// 广播游戏模式已结束
			if ( CurrentGamemode > 0 )
			{
				Map.Broadcast( 6, string.Format( plugin.Config.EndText, Plugin.GetGamemodeName( CurrentGamemode ) ) );
				CurrentGamemode = 0;
				ServerConsole.FriendlyFire = FriendlyFireDefault;
				Timing.KillCoroutines( "DelayedRoundEnd" );
			}
		}

		public void OnGrenadeThrown( ThrownProjectileEventArgs ev )
		{
			// 增加躲避球的大小并减少其引信时间
			if ( CurrentGamemode == Gamemode.Dodgeball && ev.Projectile.ProjectileType == ProjectileType.Scp018 )
			{
				ev.Projectile.Scale *= 3;
				( ev.Projectile as Scp018Projectile ).FuseTime = 1;
				ev.Player.AddItem( ItemType.SCP018 );
			}
		}

		public void OnItemDropped( DroppingItemEventArgs ev )
		{
			// 禁止丢弃躲避球，以防玩家丢失所有躲避球
			if ( CurrentGamemode == Gamemode.Dodgeball )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnGeneratorDeactivate( StoppingGeneratorEventArgs ev )
		{
			// 防止发电机在激活后被停用
			if ( CurrentGamemode == Gamemode.BlueScreenOfDeath )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnRespawn( RespawningTeamEventArgs ev )
		{
			// 禁用烦人的拟态游戏模式的重生，并更改禁闭游戏模式的出生点
			if ( CurrentGamemode == Gamemode.AnnoyingMimicry )
			{
				ev.IsAllowed = false;
			}
			else if ( CurrentGamemode == Gamemode.LockedIn )
			{
				if ( ev.Wave.Team == Team.FoundationForces )
				{
					Timing.CallDelayed( 3, () => {
						foreach( Player ply in ev.Players )
						{
							ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.FacilityGuard ).Position;
						}
					} );
				}
				else if ( ev.Wave.Team == Team.ChaosInsurgency )
				{
					Timing.CallDelayed( 3, () => {
						foreach ( Player ply in ev.Players )
						{
							List<RoleTypeId> scps = new List<RoleTypeId>() {
								RoleTypeId.Scp939, RoleTypeId.Scp049, RoleTypeId.Scp096,
								RoleTypeId.Scp173, RoleTypeId.Scp106
							};
							ply.Position = RoleExtensions.GetRandomSpawnLocation( scps.RandomItem() ).Position;
						}
					} );
				}
			}
		}

		public void OnPlayerDied( DiedEventArgs ev )
		{
			// 在烦人的拟态游戏模式中，被杀死的玩家重生为939；在感染模式中，重生为049-2
			if ( CurrentGamemode == Gamemode.AnnoyingMimicry )
			{
				Timing.CallDelayed( 3, () => ev.Player.Role.Set( RoleTypeId.Scp939 ) );
			}
			else if ( CurrentGamemode == Gamemode.Infection )
			{
				Timing.CallDelayed( 3, () => {
					ev.Player.Role.Set( RoleTypeId.Scp0492 );
					ev.Player.Scale *= 1.15f;
					ev.Player.MaxHealth = 400;
					ev.Player.Health = 400;
				} );
			}
		}

		public void OnDoorUse( InteractingDoorEventArgs ev )
		{
			// 禁用烦人的拟态和禁闭游戏模式中的大门使用
			if ( ( CurrentGamemode == Gamemode.AnnoyingMimicry || CurrentGamemode == Gamemode.LockedIn ) && ( ev.Door == Door.Get( "GATE_A" ) || ev.Door == Door.Get( "GATE_B" ) ) )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnDecon( DecontaminatingEventArgs ev )
		{
			if ( CurrentGamemode == Gamemode.LockedIn )
			{
				ev.IsAllowed = false;
			}
		}
		#endregion
	}
}
