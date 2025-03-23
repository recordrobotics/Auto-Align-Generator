using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.nt
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pose2d : IRawStruct
    {
        public Translation2d m_translation;
        public Rotation2d m_rotation;

        public readonly string TypeStr => "struct:Pose2d";

        public override string ToString()
        {
            return $"({m_translation.m_x}, {m_translation.m_y}, {m_rotation.m_value})";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Translation2d
    {
        public double m_x;
        public double m_y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rotation2d
    {
        public double m_value;
        public double m_cos;
        public double m_sin;
    }
}
