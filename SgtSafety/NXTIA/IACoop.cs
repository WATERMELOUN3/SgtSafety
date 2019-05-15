using Microsoft.Xna.Framework;
using SgtSafety.NXTEnvironment;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    class IACoop : IADijkstra
    {
        public enum Collision { PENETRE_PERIMETRE, PONCTUELLE, FRONTALE};

        private List<Point> pathRobotTelecommande;
        private List<Point> pathRobotIA;
        private List<Point> patients;
        private IADijkstra simulRobotTelec;
        private NXTVehicule robotTelec;
        private Point targetIA;
        private Point targetTelec;
        private int patientsDropped;


        // --------------------------------------------------------------------------
        // CONSTRUCTORS
        // --------------------------------------------------------------------------
        public IACoop(NXTVehicule p_robotTelec, NXTVehicule robotAuto) :
            base(robotAuto)
        {
            this.robotTelec = p_robotTelec;
            this.simulRobotTelec = new IADijkstra(p_robotTelec, true);
            this.patients = robotTelec.Circuit.Patients;
            this.patientsDropped = 0;
            this.targetTelec = NXTVehicule.ERROR;
            this.pathRobotIA = new List<Point>();

            Point ttargetTl = DetermineExcptdTelecTarget();
            Point positionTelec = robotTelec.Position;
            //Console.WriteLine("Cible telec : " + ttargetTl);
            this.pathRobotTelecommande = this.simulRobotTelec.ComputeDijkstra(positionTelec, ttargetTl); ;
        }



        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        //Retourne true si le robot telecommandé suit le chemin prévu par l'IA, retourne false sinon
        private bool TelecFollowsExcptdPath(Point position)
        {
            if (pathRobotTelecommande.Count > 0)
                return (pathRobotTelecommande.ElementAt(0) == position);

            return false;
        }

        //Retourne l'indice auxquel les chemins des 2 robots se croisent, int.MaxValue sinon
        private int PathesCross()
        {
            int minLength;
            if (pathRobotIA.Count > pathRobotTelecommande.Count)
                minLength = pathRobotTelecommande.Count;
            else
                minLength = pathRobotIA.Count;

            for (int i = 0; i < minLength; ++i)
                if (pathRobotIA.ElementAt(i).Equals(pathRobotTelecommande.ElementAt(i)))
                    return i;

            return int.MaxValue;
        }

        //Retourne le point cible attendu du robot télécommandé
        private Point DetermineExcptdTelecTarget()
        {
            if (robotTelec.Patients > 0)
                return simulRobotTelec.FindClosestHopital();
            return simulRobotTelec.FindClosestPatient(NXTVehicule.ERROR);
        }

        //Retourne le point cible de l'IA
        private Point DetermineNewTrgtIA(Point targetTelec)
        {
            if (vehicule.Patients > 0)
                return simulRobotTelec.FindClosestHopital();
            return simulRobotTelec.FindClosestPatient(targetTelec);
        }

        /*private bool TelecEntersIAPerimeter()
        {
            Point posIA = vehicule.Position, 
                  posTelec = robotTelec.Position;

            int xIA = posIA.X,
                yIA = posIA.Y,
                xTl = posTelec.X,
                yTl = posTelec.Y;

            return (xIA == xTl && (yIA == yTl + 1 || yIA == yTl - 1) || (yIA == yTl && (xIA == xTl - 1 || xIA == xTl + 1)));
        }*/

        //Retourne le type de collision (frontale ou ponctuelle)
        private Collision TypeCollision(int indexPathCross)
        {
            Point posSuivIA = pathRobotIA.ElementAt(indexPathCross + 1),
                  posSuivTelec = pathRobotTelecommande.ElementAt(indexPathCross + 1),
                  pos = pathRobotTelecommande.ElementAt(indexPathCross);
            int xSuivIA = posSuivIA.X,
                ySuivIA = posSuivIA.Y,
                xSuivTl = posSuivTelec.X,
                ySuivTl = posSuivTelec.Y,
                x = pos.X,
                y = pos.Y;

            if (!posSuivIA.Equals(posSuivTelec))
                return Collision.PONCTUELLE;
            //si le vehicule teleguide se retrouve de face face au vehicule IA
            if ((xSuivIA == xSuivTl && 
                ((ySuivIA == y-1 && ySuivTl == y+1) || (ySuivIA == y+1 && ySuivTl == y-1))) ||
                (ySuivIA == ySuivTl &&
                ((xSuivIA == x - 1 && xSuivTl == x + 1) || (xSuivIA == x + 1 && xSuivTl == x - 1))))
                return Collision.FRONTALE;

            return TypeCollision(indexPathCross + 1);
        }

        //Calcule et retourne le chemin de l'IA, en évitant les collisions avec le robot télécommand (chemin vers 1 patient puis hopital)
        private List<Point> ComputePathIAWithoutCollision(int indexPathCross)
        {
            List<Point> newPath = new List<Point>();
            Initialize(vehicule.Position);

            switch (TypeCollision(indexPathCross))
            {
                case Collision.PENETRE_PERIMETRE:
                    //OSEF
                    break;
                case Collision.PONCTUELLE:
                    pathRobotIA.Insert(0, vehicule.Position);
                    break;
                case Collision.FRONTALE:
                    this.RemoveFullNode(this.FindNodeAt(pathRobotIA[indexPathCross]));
                    newPath = ComputeDijkstra(vehicule.Position, targetIA);
                    break;
            }

            return newPath;
        }

        //Met à jour les patients si take
        private void ComputeAction(NXTAction action, Point position)
        {
            if (action.Action.Equals(NXTAction.TAKE))
                this.patients.Remove(position);
        }

        //Calcule potentiellement le nouveau path de l'IA selon le mouvement du robot télécommandé.
        public bool ComputeMove()
        {
            Point positionTelec, newTargetTelec;
            int indexPathCross;
            NXTAction action;
            bool pathIAChanged = false;

            action = robotTelec.executeCommand();
            if (pathRobotTelecommande.Count > 0)
                pathRobotTelecommande.RemoveAt(0);

            positionTelec = robotTelec.Position;
            if (action != null)
                ComputeAction(action, positionTelec);

            if (!TelecFollowsExcptdPath(positionTelec))
            {

                newTargetTelec = DetermineExcptdTelecTarget();
                pathRobotTelecommande = this.simulRobotTelec.ComputeDijkstra(positionTelec, newTargetTelec);
                vehicule.Circuit.PaintPath(pathRobotTelecommande, Color.Orange);
                if (targetTelec != newTargetTelec)
                {
                    targetTelec = newTargetTelec;
                    targetIA = DetermineNewTrgtIA(newTargetTelec);
                    //Console.WriteLine("Cible telec : " + targetTelec + "Cible IA : " + targetIA);

                    this.pathRobotIA = ComputeDijkstra(vehicule.Position, targetIA);
                    pathIAChanged = true;
                }

                indexPathCross = int.MaxValue;
                indexPathCross = PathesCross();
                if (indexPathCross != int.MaxValue)
                {
                    this.pathRobotIA = ComputePathIAWithoutCollision(indexPathCross);
                    pathIAChanged = true;
                }
            }

            if (pathIAChanged)
            {
                vehicule.ClearBuffer();
                AddToIABuffer(pathRobotIA);
            }

            action = vehicule.executeCommand();
            if (pathRobotIA.Count > 0)
                pathRobotIA.RemoveAt(0);

            if (action != null)
                ComputeAction(action, vehicule.Position);

            return patients.Count == 0;
        }

    }
}
