using AutoAlignGenerator.nt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator
{
    public class Client
    {
        private NetworkTableInstance inst;

        private uint team;
        public uint Team
        {
            get => team; set
            {
                this.team = value;
                inst.SetServerTeam(value);
            }
        }

        private Subscriber<ReadOnlyMemory<byte>> odometrySubscriber;

        public Client(uint team)
        {
            inst = NetworkTableInstance.GetDefault();
            this.team = team;

            odometrySubscriber = inst.GetRawTopic("/AdvantageKit/RealOutputs/Odometry/Robot").Subscribe();
        }

        public void Start()
        {
            inst.StartClient4("AutoAlignGenerator");
            inst.SetServerTeam(team);
            inst.SetServer("localhost");
            inst.StartDSClient();
        }

        public void Stop()
        {
            inst.StopClient();
            inst.StopDSClient();
        }

        public bool IsConnected()
        {
            return inst.IsConnected();
        }

        public Pose2d GetPose()
        {
            return odometrySubscriber.GetStruct<Pose2d>();
        }
    }
}
