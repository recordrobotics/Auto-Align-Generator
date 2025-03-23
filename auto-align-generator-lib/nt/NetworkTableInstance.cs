using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.nt
{
    public class NetworkTableInstance
    {
        private readonly NT_Inst inst;

        internal NetworkTableInstance(NT_Inst inst)
        {
            this.inst = inst;
        }

        public static NetworkTableInstance GetDefault()
        {
            return new NetworkTableInstance(NTCore.NT_GetDefaultInstance());
        }

        public void StartClient4(string identity)
        {
            WPI_String str = new WPI_String(identity);
            NTCore.NT_StartClient4(inst, ref str);
        }

        public void SetServerTeam(uint team)
        {
            NTCore.NT_SetServerTeam(inst, team);
        }

        public void SetServerTeam(uint team, uint port)
        {
            NTCore.NT_SetServerTeam(inst, team, port);
        }

        public void StartDSClient()
        {
            NTCore.NT_StartDSClient(inst);
        }

        public void StartDSClient(uint port)
        {
            NTCore.NT_StartDSClient(inst, port);
        }

        public void StopClient()
        {
            NTCore.NT_StopClient(inst);
        }

        public void StopDSClient()
        {
            NTCore.NT_StopDSClient(inst);
        }

        public bool IsConnected()
        {
            return NTCore.NT_IsConnected(inst);
        }

        public Topic<double> GetDoubleTopic(string name)
        {
            WPI_String str = new WPI_String(name);
            return new Topic<double>(NTCore.NT_GetTopic(inst, ref str), NT_Type.NT_DOUBLE);
        }

        public Topic<ReadOnlyMemory<byte>> GetRawTopic(string name)
        {
            WPI_String str = new WPI_String(name);
            return new Topic<ReadOnlyMemory<byte>>(NTCore.NT_GetTopic(inst, ref str), NT_Type.NT_RAW);
        }
    }
}
