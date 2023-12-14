using System;
using Life;
using Life.Network;
using Life.VehicleSystem;

namespace FoxTweaks.TweakClass
{
    public class SellEnterpriseCarFox
    {
        private const string TitleWarningMessage = "Vente de voiture";

        public SellEnterpriseCarFox()
        {
            var sChatCommand = new SChatCommand("/sellCar", "Vendre le véhicule de société",
                "/sellCar", (player, arg) =>
                {
                    SellCar(player);
                });
            sChatCommand.Register();
        }
        
        private static void SellCar(Player player)
        {
            var closestVehicle = player.GetClosestVehicle();
            if (!player.HasBiz()) return;
            if (player.biz.IsActivity(Life.BizSystem.Activity.Type.Mecanic))
            {
                if (closestVehicle != null)
                {
                    var vToSell = Nova.v.GetVehicle(closestVehicle.vehicleDbId);
                    if (vToSell.bizId == player.biz.Id)
                    {
                        var playerBuyer = player.GetClosestPlayer();
                        if (playerBuyer != null)
                        {
                            vToSell.bizId = 0;
                            vToSell.permissions.coOwners.Clear();
                            vToSell.permissions.owner.characterId = playerBuyer.character.Id;
                            vToSell.SaveAndFake();

                            player.Notify(TitleWarningMessage,
                                "Propriété du véhicule transféré à " + playerBuyer.GetFullName(),
                                NotificationManager.Type.Success);

                            playerBuyer.Notify(TitleWarningMessage, "Propriété du véhicule : " + vToSell.plate + " aquise !",
                                NotificationManager.Type.Success);
                        }
                        else
                        {
                            player.Notify(TitleWarningMessage,
                                "Aucun client n'est près de vous pour acheter le véhicule.", NotificationManager.Type.Error);
                        }
                    }
                    else
                    {
                        player.Notify(TitleWarningMessage,
                            "Ce véhicule n'est pas un véhicule de votre société.", NotificationManager.Type.Error);
                    }
                }
                else
                {
                    player.Notify(TitleWarningMessage, "Aucun véhicule près de vous à vendre.", NotificationManager.Type.Error);
                }
            }
            else
            {
                player.Notify(TitleWarningMessage, "Vous n'êtes mecanicien.", NotificationManager.Type.Error);
            }
        }
    }
}