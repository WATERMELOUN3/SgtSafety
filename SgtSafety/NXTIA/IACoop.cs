using Microsoft.Xna.Framework;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    class IACoop : IADijkstra
    {
        private List<Point> pathRobotTelecommande;
        private List<Point> pathRobotIA;
        private NXTVehicule robotTelec;
        private Point targetIA;
        private Point targetTelec;


        // --------------------------------------------------------------------------
        // CONSTRUCTORS
        // --------------------------------------------------------------------------
        public IACoop(NXTVehicule p_robotTelec, NXTVehicule robotAuto) :
            base(robotAuto)
        {
            this.robotTelec = p_robotTelec;
        }



        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        //Retourne true si le robot telecommandé suit le chemin prévu par l'IA, retourne false sinon
        private bool TelecFollowsExcptdPath(Point position)
        {
            if (pathRobotTelecommande.Count > 0)
                return (pathRobotTelecommande.ElementAt(0) == position);

            return true;
        }

        //Retourne l'indice auxquel les chemins des 2 robots se croisent, int.MaxValue sinon
        private int PathesCross()
        {
            int minLenght;
            if (pathRobotIA.Count > pathRobotTelecommande.Count)
                minLenght = pathRobotTelecommande.Count;
            else
                minLenght = pathRobotIA.Count;

            for (int i = 0; i < minLenght; ++i)
                if (pathRobotIA.ElementAt(i).Equals(pathRobotTelecommande.ElementAt(i)))
                    return i;

            return int.MaxValue;
        }

        //Retourne le point cible attendu du robot télécommandé
        private Point FindExcptdTelecTarget()
        {
            return new Point();
        }

        private Point DetermineNewTrgtIA()
        {
            return new Point();
        }

        //Calcule potentiellement le nouveau path de l'IA selon le mouvement du robot télécommandé.
        public void ComputeMove()
        {
            Point positionTelec, newTargetTelec;
            int indexPathCross;

            robotTelec.executeCommand();
            positionTelec = robotTelec.Position;
            if (!TelecFollowsExcptdPath(positionTelec))
            {

                newTargetTelec = FindExcptdTelecTarget();
                //pathRobotTelecommande = ComputePathDijkstra(robotTelec, newTargetTelec);
                if (targetTelec != newTargetTelec)
                {
                    targetTelec = newTargetTelec;
                    targetIA = DetermineNewTrgtIA();
                    vehicule.ClearBuffer();
                    // this.pathRobotIA = 
                }

                indexPathCross = PathesCross();
                if (indexPathCross != int.MaxValue)
                {
                    vehicule.ClearBuffer();
                    // this.pathRobotIA = 
                }
            }

            AddToIABuffer(pathRobotIA);
            //sendToVehiculeBuffer();
            vehicule.executeCommand();
        }
    }
}
