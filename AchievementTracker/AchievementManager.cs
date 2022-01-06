using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker
{
    public static class AchievementManager
    {
        public static void GetStats()
        {

        }

        public static void Earn(Achievements.Type achievement)
        {
            OnEarn(achievement.ToString(), GetAchievementInfo(achievement));
        }

        public static void EarnCustom(string uniqueID)
        {
                  
        }

        private static void OnEarn(string uniqueID, AchievementInfo info)
        {
            Logger.Log($"Earned achievement {info.Name} : {info.Description}");
            AchievementData.EarnAchievement(uniqueID);
        }

        public static AchievementInfo GetAchievementInfo(Achievements.Type type)
        {
            string name;
            string description;
            switch (type)
            {
                case Achievements.Type.TERRIBLE_FATE:
                    name = "You've met a terrible fate.";
                    description = "No going back.";
                    break;
                case Achievements.Type.WHATS_THIS_BUTTON:
                    name = "Hey, what's this button do?";
                    description = "Press the ejection button in the ship.";
                    break;
                case Achievements.Type.ALPHA_PILOT:
                    name = "Hotshot";
                    description = "Manually fly to the Sun Station.";
                    break;
                case Achievements.Type.YOU_TRIED:
                    name = "It was worth a shot.";
                    description = "You tried to escape the solar system.";
                    break;
                case Achievements.Type.BEGINNERS_LUCK:
                    name = "Beginner's Luck";
                    description = "Reach the Eye of the Universe in one loop.";
                    break;
                case Achievements.Type.SATELLITE:
                    name = "Rigidbody";
                    description = "De-orbit the Hearthian satellite.";
                    break;
                case Achievements.Type.HEARTH_TO_MOON:
                    name = "From the Hearth to the Moon";
                    description = "Land the model rocket on Attlerock.";
                    break;
                case Achievements.Type.DEEP_IMPACT:
                    name = "Deep Impact";
                    description = "Enter Giant's Deep ocean fast enough to break through the current.";
                    break;
                case Achievements.Type.HARMONIC_CONVERGENCE:
                    name = "Harmonic Convergence";
                    description = "Pick up all traveler instruments simultaneously with your signalscope.";
                    break;
                case Achievements.Type.MUSEUM:
                    name = "It belongs in a museum!";
                    description = "Bring an artifact back to the museum.";
                    break;
                case Achievements.Type.DIEHARD:
                    name = "Die Hard";
                    description = "End a time loop (Alive) after having taken (and healed) over 1000 damage.";
                    break;
                case Achievements.Type.PCHOOOOOOO:
                    name = "Pchooooooo!";
                    description = "Launch your ship with a gravity cannon.";
                    break;
                case Achievements.Type.GONE_IN_60_SECONDS:
                    name = "Gone In 60 Seconds";
                    description = "Die within 60 seconds of waking up.";
                    break;
                case Achievements.Type.CARCINOGENS:
                    name = "Mmmm, Carcinogens...";
                    description = "Eat 10 burnt marshmallows.";
                    break;
                case Achievements.Type.CUTTING_IT_CLOSE:
                    name = "Cutting it Close";
                    description = "Use your suit's oxygen in an ill-advised manner.";
                    break;
                case Achievements.Type.MICAS_WRATH:
                    name = "Mica's Wrath";
                    description = "Destroy the Model Rocket by flying into the sun or Hollow's Lantern.";
                    break;
                case Achievements.Type.STUDIOUS:
                    name = "Archaeologist";
                    description = "Complete the ship log.";
                    break;
                case Achievements.Type.AROUND_THE_WORLD:
                    name = "Around the world in 90 seconds";
                    description = "Raft around the Stranger in under 90 seconds.";
                    break;
                case Achievements.Type.SILENCED_CARTOGRAPHER:
                    name = "The Silenced Cartographer";
                    description = "Render the Deep Space Satellite inoperable.";
                    break;
                case Achievements.Type.TUBULAR:
                    name = "Tubular!";
                    description = "Ride the face of the wave for at least 15 seconds on a raft.";
                    break;
                case Achievements.Type.EARLY_ADOPTER:
                    name = "Early Adopter";
                    description = "Attempt to use the second artifact prototype. Curiosity killed the cat, but a time loop brought it back.";
                    break;
                case Achievements.Type.GRATE_FILTER:
                    name = "The Grate Filter";
                    description = "Swim through the dam grate. Not everyone makes it!";
                    break;
                case Achievements.Type.FLAT_HEARTHER:
                    name = "Flat Hearther";
                    description = "Stand your ground beneath a moving chain elevator.";
                    break;
                case Achievements.Type.CELCIUS:
                    name = "Celsius 232.78";
                    description = "This knowledge isn't going to forbid itself!";
                    break;
                case Achievements.Type.GHOSTS:
                    name = "Ghosts in the Machine";
                    description = "Reach all 3 Forbidden Archives in a single loop without getting caught.";
                    break;
                case Achievements.Type.SLEEP_WAKE_REPEAT:
                    name = "Sleep. Wake. Repeat.";
                    description = "Be woken up from a dream 5 different ways in a single loop.";
                    break;
                case Achievements.Type.SIMULATION:
                    name = "Simulation Hypothesis";
                    description = "Attempt to use the artifact at a normal campfire. Well, that settles that!";
                    break;
                case Achievements.Type.FIRE_ARROWS:
                    name = "Fire Arrows";
                    description = "Shoot the Little Scout at the artificial sun in the Stranger.";
                    break;
                case Achievements.Type.ONE_NINE:
                    name = "1 / 900";
                    description = "Yahaha! You found me!";
                    break;
                case Achievements.Type.TAKEMEALIVE:
                    name = "You'll Never Take Me Alive!";
                    description = "Escape your pursuers by jumping to your \"death\" instead of dodging past them.";
                    break;
                case Achievements.Type.OOFMYBONES:
                    name = "Oof Ouch, My Bones";
                    description = "Have your spine adjusted by a pursuer.";
                    break;
                default:
                    name = "Wait what";
                    description = "This is impossible, contact xen and tell me why this happened";
                    break;
            }

            return new AchievementInfo(name, description);
        }

        public class AchievementInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public AchievementInfo(string name, string description)
            {
                Name = name;
                Description = description;
            }
        }
    }
}
