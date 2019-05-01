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

        //Retourne le patient le plus proche du robot, le seconde argument n'est à passer que dans le cas de l'IA, sinon lui passer NXTVehicule.ERROR
        private Point FindClosestPatient(NXTVehicule source, Point targetTelec)
        {
            int distanceMin = int.MaxValue, 
                distance;
            Point closestPatient = NXTVehicule.ERROR;
            foreach (Point p in this.patients)
            {
                distance = GetManhattanHeuristic(p, source.Position);
                if (distance < distanceMin && !p.Equals(targetTelec))
                {
                    distanceMin = distance;
                    closestPatient = p;
                }
            }

            return closestPatient;
        }

        //Retourne l'hopital le plus proche du robot
        private Point FindClosestHopital(NXTVehicule source)
        {
            List<Point> hopitaux = robotTelec.Circuit.Patients;
            int distanceMin = int.MaxValue,
                distance;
            Point closestHopital = NXTVehicule.ERROR;
            foreach (Point h in hopitaux)
            {
                distance = GetManhattanHeuristic(h, source.Position);
                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    closestHopital = h;
                }
            }

            return closestHopital;
        }

        //Retourne le point cible attendu du robot télécommandé
        private Point DetermineExcptdTelecTarget()
        {
            if (robotTelec.Patients > 0)
                return FindClosestHopital(robotTelec);
            return FindClosestPatient(robotTelec, NXTVehicule.ERROR);
        }

        //Retourne le point cible de l'IA
        private Point DetermineNewTrgtIA(Point targetTelec)
        {
            if (vehicule.Patients > 0)
                return FindClosestHopital(vehicule);
            return FindClosestPatient(vehicule, targetTelec);
        }

        //Calcule et retourne le chemin de l'IA, en évitant les collisions avec le robot télécommand (chemin vers 1 patient puis hopital)
        private List<Point> ComputePathIAWithoutCollision()
        {
            List<Point> newPath = new List<Point>();

            return newPath;
        }

        //Met à jour les patients si take
        private void ComputeAction(NXTAction action, Point position)
        {
            if (action.Action.Equals(NXTAction.TAKE))
                this.patients.Remove(position);
        }

        //Calcule potentiellement le nouveau path de l'IA selon le mouvement du robot télécommandé.
        private void ComputeMove()
        {
            Point positionTelec, newTargetTelec;
            int indexPathCross;
            NXTAction action;

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
                if (targetTelec != newTargetTelec)
                {
                    targetTelec = newTargetTelec;
                    targetIA = DetermineNewTrgtIA(targetTelec);

                    vehicule.ClearBuffer();
                    this.pathRobotIA = ComputePathIAWithoutCollision();
                }

                indexPathCross = int.MaxValue;
                indexPathCross = PathesCross();
                if (indexPathCross != int.MaxValue)
                {
                    vehicule.ClearBuffer();
                    this.pathRobotIA = ComputePathIAWithoutCollision();
                }
            }

            AddToIABuffer(pathRobotIA);
            sendToVehiculeBuffer();

            action = vehicule.executeCommand();
            pathRobotIA.RemoveAt(0);
            ComputeAction(action, vehicule.Position);
        }

        public void MainLoop()
        {
            int nbPatientsStart = patients.Count;
            while (patientsDropped < nbPatientsStart)
            {
                ComputeMove();
            }
        }
    }
}
