using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileHelpers;

namespace PlotDVT
{
    [DelimitedRecord(",")]
    public class RealPowerAnswers
    {
        public string TIMESTAMP;
        [FieldValueDiscarded]
        public string cfg_ac_apparent_power;
        //[FieldValueDiscarded]
        public string cfg_ac_frequency;
        //[FieldValueDiscarded]
        public string cfg_ac_line_to_line_voltage;
       // [FieldValueDiscarded]
        public string cfg_ac_reactive_power;
        //[FieldValueDiscarded]
        public string cfg_ac_real_power;
        //[FieldValueDiscarded]
        public string cfg_ac_voltage;
        //[FieldValueDiscarded]
        public string cfg_assumed_efficiency;
        //[FieldValueDiscarded]
        public string cfg_dc_current;
        //[FieldValueDiscarded]
        public string cfg_dc_power;
        [FieldValueDiscarded]
        public string cfg_dc_voltage;
        [FieldValueDiscarded]
        public string cfg_dreq;
        [FieldValueDiscarded]
        public string cfg_fill_factor;
        [FieldValueDiscarded]
        public string cfg_frac;
        [FieldValueDiscarded]
        public string cfg_ireq;
        [FieldValueDiscarded]
        public string cfg_pcu_type_name;
        [FieldValueDiscarded]
        public string cfg_phase;
        [FieldValueDiscarded]
        public string cfg_power_ratio;
        [FieldValueDiscarded]
        public string cfg_qreq;
        [FieldValueDiscarded]
        public string cfg_sn;
        [FieldValueDiscarded]
        public string cfg_temperature;
        [FieldValueDiscarded]
        public string pcu_qM_fDCRL;
        [FieldValueDiscarded]
        public string pcu_qM_fFOOR;
        [FieldValueDiscarded]
        public string pcu_qM_fGRID;
        [FieldValueDiscarded]
        public string pcu_qM_fIACH;
        [FieldValueDiscarded]
        public string pcu_qM_fIACL;
        [FieldValueDiscarded]
        public string pcu_qM_fROC;
        [FieldValueDiscarded]
        public string pcu_qM_fVACA;
        [FieldValueDiscarded]
        public string pcu_qM_fVDCH;
        [FieldValueDiscarded]
        public string pcu_qM_fVDCL;
        [FieldValueDiscarded]
        public string pcu_qM_fVL1;
        [FieldValueDiscarded]
        public string pcu_qP_ACCIMAG;
        [FieldValueDiscarded]
        public string pcu_qP_ACCREAL;
        [FieldValueDiscarded]
        public string pcu_qP_ACW;
        [FieldValueDiscarded]
        public string pcu_qP_ACWDREQ;
        [FieldValueDiscarded]
        public string pcu_qP_ACWIMAG;
        [FieldValueDiscarded]
        public string pcu_qP_ACWQREQ;
        [FieldValueDiscarded]
        public string pcu_qP_ACWREAL;
        [FieldValueDiscarded]
        public string pcu_qP_AI;
        [FieldValueDiscarded]
        public string pcu_qP_DCW;
        [FieldValueDiscarded]
        public string pcu_qP_DREQ;
        [FieldValueDiscarded]
        public string pcu_qP_ENER;
        [FieldValueDiscarded]
        public string pcu_qP_FREQ;
        [FieldValueDiscarded]
        public string pcu_qP_IAC;
        [FieldValueDiscarded]
        public string pcu_qP_IDC;
        [FieldValueDiscarded]
        public string pcu_qP_NOFF;
        [FieldValueDiscarded]
        public string pcu_qP_PF;
        [FieldValueDiscarded]
        public string pcu_qP_QREQ;
        [FieldValueDiscarded]
        public string pcu_qP_SPS;
        [FieldValueDiscarded]
        public string pcu_qP_TEMP;
        [FieldValueDiscarded]
        public string pcu_qP_TIME;
        [FieldValueDiscarded]
        public string pcu_qP_VA;
        //[FieldValueDiscarded]
        public string pcu_qP_VDC;
        [FieldValueDiscarded]
        public string pcu_qP_VL1;
        [FieldValueDiscarded]
        public string pcu_qP_VL12;
        [FieldValueDiscarded]
        public string pcu_qS_AAI;
        [FieldValueDiscarded]
        public string pcu_qS_AI;
        [FieldValueDiscarded]
        public string pcu_qS_AMT;
        [FieldValueDiscarded]
        public string pcu_qS_DCA;
        [FieldValueDiscarded]
        public string pcu_qS_DVDT;
        [FieldValueDiscarded]
        public string pcu_qS_ETC;
        [FieldValueDiscarded]
        public string pcu_qS_FRER;
        [FieldValueDiscarded]
        public string pcu_qS_HIGHV;
        [FieldValueDiscarded]
        public string pcu_qS_HSD;
        [FieldValueDiscarded]
        public string pcu_qS_LOWV;
        [FieldValueDiscarded]
        public string pcu_qS_PLER;
        [FieldValueDiscarded]
        public string pcu_qS_PLL;
        [FieldValueDiscarded]
        public string pcu_qS_PLOK;
        [FieldValueDiscarded]
        public string pcu_qS_PSE1;
        [FieldValueDiscarded]
        public string pcu_qS_PSE2;
        [FieldValueDiscarded]
        public string pcu_qS_ROCN;
        [FieldValueDiscarded]
        public string pcu_qS_ROCP;
        [FieldValueDiscarded]
        public string pcu_qS_SPO;
        [FieldValueDiscarded]
        public string pcu_qS_SSO;
        [FieldValueDiscarded]
        public string pcu_qS_STO;
        [FieldValueDiscarded]
        public string pcu_qS_SUSP;
        [FieldValueDiscarded]
        public string pcu_qS_SW;
        [FieldValueDiscarded]
        public string pcu_qS_UNIS;
        [FieldValueDiscarded]
        public string pcu_qS_UO;
        [FieldValueDiscarded]
        public string pcu_qS_ZXVH;
        [FieldValueDiscarded]
        public string pm_ac_1_m_peak_amps;
        [FieldValueDiscarded]
        public string pm_ac_1_p_peak_amps;
        [FieldValueDiscarded]
        public string pm_ac_2_m_peak_amps;
        [FieldValueDiscarded]
        public string pm_ac_2_p_peak_amps;
        [FieldValueDiscarded]
        public string pm_ac_amp_hours;
        [FieldValueDiscarded]
        public string pm_ac_amps;
        [FieldValueDiscarded]
        public string pm_ac_amps_1;
        [FieldValueDiscarded]
        public string pm_ac_amps_2;
        [FieldValueDiscarded]
        public string pm_ac_deg;
        [FieldValueDiscarded]
        public string pm_ac_frequency;
        [FieldValueDiscarded]
        public string pm_ac_lambda;
        [FieldValueDiscarded]
        public string pm_ac_va;
        [FieldValueDiscarded]
        public string pm_ac_var;
        [FieldValueDiscarded]
        public string pm_ac_volts;
        [FieldValueDiscarded]
        public string pm_ac_volts_1;
        [FieldValueDiscarded]
        public string pm_ac_volts_2;
        [FieldValueDiscarded]
        public string pm_ac_watt_hours;
        [FieldValueDiscarded]
        public string pm_ac_watts;
        [FieldValueDiscarded]
        public string pm_dc_amp_hours_1;
        [FieldValueDiscarded]
        public string pm_dc_amp_hours_2;
        [FieldValueDiscarded]
        public string pm_dc_amps_1;
        [FieldValueDiscarded]
        public string pm_dc_amps_2;
        [FieldValueDiscarded]
        public string pm_dc_m_peak_amps_1;
        [FieldValueDiscarded]
        public string pm_dc_m_peak_amps_2;
        [FieldValueDiscarded]
        public string pm_dc_m_peak_volts_1;
        [FieldValueDiscarded]
        public string pm_dc_m_peak_volts_2;
        [FieldValueDiscarded]
        public string pm_dc_p_peak_amps_1;
        [FieldValueDiscarded]
        public string pm_dc_p_peak_amps_2;
        [FieldValueDiscarded]
        public string pm_dc_p_peak_volts_1;
        [FieldValueDiscarded]
        public string pm_dc_p_peak_volts_2;
        //[FieldValueDiscarded]
        public string pm_dc_volts_1;
        //[FieldValueDiscarded]
        public string pm_dc_volts_2;
        [FieldValueDiscarded]
        public string pm_dc_watt_hours_1;
        [FieldValueDiscarded]
        public string pm_dc_watt_hours_2;
        [FieldValueDiscarded]
        public string pm_dc_watts_1;
        [FieldValueDiscarded]
        public string pm_dc_watts_2;
        [FieldValueDiscarded]
        public string pm_eff;
        [FieldValueDiscarded]
        public string pm_eff_1;
        [FieldValueDiscarded]
        public string pm_eff_2;
        [FieldValueDiscarded]
        public string pm_harmonic1;
        [FieldValueDiscarded]
        public string pm_harmonic2;
        [FieldValueDiscarded]
        public string pm_harmonic3;
        [FieldValueDiscarded]
        public string pm_harmonic4;
        [FieldValueDiscarded]
        public string pm_harmonic5;
        [FieldValueDiscarded]
        public string pm_harmonic6;
        [FieldValueDiscarded]
        public string pm_harmonic7;
        [FieldValueDiscarded]
        public string pm_harmonic8;
        [FieldValueDiscarded]
        public string pm_harmonic9;
        [FieldValueDiscarded]
        public string pm_harmonic10;
        [FieldValueDiscarded]
        public string pm_harmonic11;
        [FieldValueDiscarded]
        public string pm_harmonic12;
        [FieldValueDiscarded]
        public string pm_harmonic13;
        [FieldValueDiscarded]
        public string pm_harmonic14;
        [FieldValueDiscarded]
        public string pm_harmonic15;
        [FieldValueDiscarded]
        public string pm_harmonic16;
        [FieldValueDiscarded]
        public string pm_harmonic17;
        [FieldValueDiscarded]
        public string pm_harmonic18;
        [FieldValueDiscarded]
        public string pm_harmonic19;
        [FieldValueDiscarded]
        public string pm_harmonic20;
        [FieldValueDiscarded]
        public string pm_harmonic21;
        [FieldValueDiscarded]
        public string pm_harmonic22;
        [FieldValueDiscarded]
        public string pm_harmonic23;
        [FieldValueDiscarded]
        public string pm_harmonic24;
        [FieldValueDiscarded]
        public string pm_harmonic25;
        [FieldValueDiscarded]
        public string pm_harmonic26;
        [FieldValueDiscarded]
        public string pm_harmonic27;
        [FieldValueDiscarded]
        public string pm_harmonic28;
        [FieldValueDiscarded]
        public string pm_harmonic29;
        [FieldValueDiscarded]
        public string pm_harmonic30;
        [FieldValueDiscarded]
        public string pm_harmonic31;
        [FieldValueDiscarded]
        public string pm_harmonic32;
        [FieldValueDiscarded]
        public string pm_harmonic33;
        [FieldValueDiscarded]
        public string pm_harmonic34;
        [FieldValueDiscarded]
        public string pm_harmonic35;
        [FieldValueDiscarded]
        public string pm_harmonic36;
        [FieldValueDiscarded]
        public string pm_harmonic37;
        [FieldValueDiscarded]
        public string pm_harmonic38;
        [FieldValueDiscarded]
        public string pm_harmonic39;
        [FieldValueDiscarded]
        public string pm_harmonic40;
        [FieldValueDiscarded]
        public string pm_phi1;
        [FieldValueDiscarded]
        public string pm_phi2;
        [FieldValueDiscarded]
        public string pm_phi3;
        [FieldValueDiscarded]
        public string pm_phi4;
        [FieldValueDiscarded]
        public string pm_phi5;
        [FieldValueDiscarded]
        public string pm_phi6;
        [FieldValueDiscarded]
        public string pm_phi7;
        [FieldValueDiscarded]
        public string pm_phi8;
        [FieldValueDiscarded]
        public string pm_phi9;
        [FieldValueDiscarded]
        public string pm_phi10;
        [FieldValueDiscarded]
        public string pm_phi11;
        [FieldValueDiscarded]
        public string pm_phi12;
        [FieldValueDiscarded]
        public string pm_phi13;
        [FieldValueDiscarded]
        public string pm_phi14;
        [FieldValueDiscarded]
        public string pm_phi15;
        [FieldValueDiscarded]
        public string pm_phi16;
        [FieldValueDiscarded]
        public string pm_phi17;
        [FieldValueDiscarded]
        public string pm_phi18;
        [FieldValueDiscarded]
        public string pm_phi19;
        [FieldValueDiscarded]
        public string pm_phi20;
        [FieldValueDiscarded]
        public string pm_phi21;
        [FieldValueDiscarded]
        public string pm_phi22;
        [FieldValueDiscarded]
        public string pm_phi23;
        [FieldValueDiscarded]
        public string pm_phi24;
        [FieldValueDiscarded]
        public string pm_phi25;
        [FieldValueDiscarded]
        public string pm_phi26;
        [FieldValueDiscarded]
        public string pm_phi27;
        [FieldValueDiscarded]
        public string pm_phi28;
        [FieldValueDiscarded]
        public string pm_phi29;
        [FieldValueDiscarded]
        public string pm_phi30;
        [FieldValueDiscarded]
        public string pm_phi31;
        [FieldValueDiscarded]
        public string pm_phi32;
        [FieldValueDiscarded]
        public string pm_phi33;
        [FieldValueDiscarded]
        public string pm_phi34;
        [FieldValueDiscarded]
        public string pm_phi35;
        [FieldValueDiscarded]
        public string pm_phi36;
        [FieldValueDiscarded]
        public string pm_phi37;
        [FieldValueDiscarded]
        public string pm_phi38;
        [FieldValueDiscarded]
        public string pm_phi39;
        [FieldValueDiscarded]
        public string pm_phi40;
        [FieldValueDiscarded]
        public string test_name;
        //[FieldOptional]//something wrong in csv 
       // public string x2;

    }
}
