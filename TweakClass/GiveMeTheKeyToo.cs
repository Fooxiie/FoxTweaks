using System;
using Life;
using Life.Network;
using Life.VehicleSystem;

namespace FoxTweaks.TweakClass
{
    public class GiveMeTheKeyToo
    {
        public GiveMeTheKeyToo()
        {
            var sChatCommand = new SChatCommand("/giveKey",
                "Donner la copropriété à la personne la plus proche de votre voiture",
                "/giveKey", (player, arg) =>
                {
                    GiveCoPro(player);
                });
            sChatCommand.Register();
        }
        
        private static void GiveCoPro(Player player)
        {
            var closestPlayer = player.GetClosestPlayer();
            if (closestPlayer != null)
            {
                var closestVehicle = player.GetClosestVehicle();
                if (closestVehicle != null)
                {
                    var vehicle = Nova.v.GetVehicle(closestVehicle.vehicleDbId);

                    var ownerID = vehicle.permissions.owner.characterId;

                    if (ownerID == player.character.Id)
                    {
                        vehicle.AddCoOwner(new Life.PermissionSystem.Entity
                        {
                            characterId = closestPlayer.character.Id,
                        });

                        vehicle.SaveAndFake();

                        player.Notify("CoPropiétaire ajouté",
                            $"Vous avez donnez un double des cléfs à {closestPlayer.GetFullName()}",
                            NotificationManager.Type.Success);

                        closestPlayer.Notify("CoPropiétaire ajouté",
                            $"Vous avez reçus la copropriété du véhicule : {closestVehicle.plate}");

                        foreach (var aroundPlayer in Nova.closestPlayers)
                        {
                            aroundPlayer.CmdSendText("<color=#f368e0>" + player.GetFullName() + " a donné un double des clefs à " + closestPlayer.GetFullName() + "</color>");
                        }
                        
                    } else
                    {
                        player.Notify("Erreur", "Le véhicule ne vous appartient pas.", NotificationManager.Type.Error);
                    }
                } else
                {
                    player.Notify("Erreur", "Aucun véhicule qui vous appartient dans les parages", NotificationManager.Type.Error);
                }
            } else
            {
                player.Notify("Erreur", "Personne n'est près de vous.", NotificationManager.Type.Error);
            }
        }
    }
}