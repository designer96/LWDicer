using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_System.EObjectLayer;

namespace LWDicer.Control
{
    public class MSystemInfo
    {
        CObjectInfo[] arrayObjectInfo;

        public MSystemInfo()
        {
            arrayObjectInfo = new CObjectInfo[]
            {
                // 0-39 : Common & Hardware
                new CObjectInfo( (int)OBJ_SYSTEM, "System", 0, "MLWDicer", 0, "System", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_DATAMANAGER, "DataManger", 1, "DataManager", 500, "DataManager", LOG_ALL, LOG_DAY ),

	            new CObjectInfo( (int)OBJ_HL_IO, "IO", 2, "Device Net", 1000, "IO", LOG_ALL, LOG_DAY ),	
		
	            new CObjectInfo( (int)OBJ_HL_MOTION_LIB, "MotionLib", 3, "MMC Board", 2000, "MotionLib", LOG_ALL, LOG_DAY ),
		
	            new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 4, "Vision", 3000, "Vision", LOG_ALL, LOG_DAY ),
		
	            new CObjectInfo( (int)OBJ_HL_MELSEC, "Melsec", 5, "Melsec", 7000, "Melsec", LOG_ALL, LOG_DAY ),
		
	            // 30-39 : Serial ------------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 10, "RS232 SHead1", 5000, "RS232_SHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 11, "RS232 SHead2", 5000, "RS232_SHead2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 12, "RS232 GHead1", 5000, "RS232_GHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 13, "RS232 GHead2", 5000, "RS232_GHead2", LOG_ALL, LOG_DAY ),
		
	            // 40-49 : Dummy Reserved
	            // 50-59 : Ethernet Reserved
	            // 60-69 : BarCode Reserved
	            // 70-79 : Melsec
		
	            // 80-99 : Reserved
		
	            // 100-149 : Cylinders--------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 100, "Unload Handler Up/Down Cylinder", 8000, "UnloadHandlerUD", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 101, "Unload Handler Up/Down Cylinder2", 8000, "UnloadHandlerUD2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 102, "SHead1 HeightBase Forward/Backward Cylinder", 8000, "HeightBase1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 103, "SHead1 HeightBase Forward/Backward Cylinder", 8000, "HeightBase2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 104, "SHead1 UVCheck Cyl1 Forward/Backward Cylinder", 8000, "SHead1UVCheckCyl1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 105, "SHead1 UVCheck Cyl2 Forward/Backward Cylinder", 8000, "SHead1UVCheckCyl2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 106, "SHead1 UVCheck Cyl3 Forward/Backward Cylinder", 8000, "SHead1UVCheckCyl3", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 107, "SHead2 UVCheck Cyl1 Forward/Backward Cylinder", 8000, "SHead2UVCheckCyl1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 108, "SHead2 UVCheck Cyl2 Forward/Backward Cylinder", 8000, "SHead2UVCheckCyl2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 109, "SHead2 UVCheck Cyl3 Forward/Backward Cylinder", 8000, "SHead2UVCheckCyl3", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 110, "GHead1 UVCheck Cyl1 Forward/Backward Cylinder", 8000, "GHead1UVCheckCyl1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 111, "GHead2 UVCheck Cyl1 Forward/Backward Cylinder", 8000, "GHead2UVCheckCyl1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 112, "SHead1 UVCheck Cyl1 Up/Down Cylinder", 8000, "SHead1UVCheckCylUD", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 113, "SHead2 UVCheck Cyl1 Up/Down Cylinder", 8000, "SHead2UVCheckCylUD", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 114, "SHead1 Cleaner Up/Down Cylinder", 8000, "Cleaner", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 115, "SHead2 Cleaner Up/Down Cylinder", 8000, "Cleaner", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 116, "GHead1 Cleaner Up/Down Cylinder", 8000, "Cleaner", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 117, "GHead2 Cleaner Up/Down Cylinder", 8000, "Cleaner", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 118, "Camera1 Up/Down Cylinder", 8000, "Camera1UD", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 119, "Camera2 Up/Down Cylinder", 8000, "Camera2UD", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 120, "Workbench Forward/Backward Cylinder", 8000, "WorkbenchFB", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 121, "Workbench Up/Down Cylinder", 8000, "WorkbenchUD", LOG_ALL, LOG_DAY ),
		
	            // 150-199 : Vacuums ------------------------------------------------------------------------------	
	            new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 150, "Stage1 Vacuum", 9000, "Stage1Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 151, "Stage2 Vacuum", 9000, "Stage2Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 152, "Stage3 Vacuum", 9000, "Stage3Vac", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 153, "Workbench Inner Vacuum", 9000, "WorkbenchInnerVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 154, "Workbench Outer Vacuum", 9000, "WorkbenchOuterVac", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 155, "UHandler Self Vacuum", 9000, "UHandlerSelfVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 156, "UHandler Factory Vacuum", 9000, "UHandlerFactoryVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 157, "UHandler Extra Vacuum", 9000, "UHandlerExtraVac", LOG_ALL, LOG_DAY ),
		
	            // 200-209 : Scanner & Laser  -------------------------------------------------------------	
                new CObjectInfo( (int)OBJ_ML_POLYGON,   "PolygonScanner",    200, "Polygon Scanner",  6000, "PolygonScanner",  LOG_ALL, LOG_DAY ),
	            // 210-249 : Multi Actuator, Induction Motor, etc Reserved -------------------------------------------------------------

	            // 250-299 : Motion Multi Axes --------------------------------------------------------------------		
	            new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes Stage1",   250, "Stage1 Motion", 11000, "Stage1Axis", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes Stage1",   251, "Stage2 Motion", 11000, "Stage2Axis", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes Stage1",   252, "Stage3 Motion", 11000, "Stage3Axis", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes Camera1",  253, "Camera1 Motion", 11000, "Camera1Axis", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes Camera2",  254, "Camera2 Motion", 11000, "Camera2Axis", LOG_ALL, LOG_DAY ),	
	            new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes SHead1",   255, "SHead1 Motion", 11000, "SHead1Axis", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes SHead2",   256, "SHead2 Motion", 11000, "SHead2Axis", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes GHead1",   257, "GHead1 Motion", 11000, "GHead1Axis", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes GHead2",   258, "GHead2 Motion", 11000, "GHead2Axis", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_HL_MULTI_AXES, "MultiAxes UHandler", 259, "UHandler Motion", 11000, "UHandlerAxis", LOG_ALL, LOG_DAY ),
		
	            // 300-349 : Mechanical Layer --------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_ML_LIGHTENING, "Lightening", 300, "Mechanical : Lightening", 12000, "Lightening", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_ML_OP_PANEL  , "OpPanel",    301, "Mechanical : Operation Panel", 14000, "OpPanel", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLamp1",    302, "Mechanical : UVLamp1", 23000, "UVLamp1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLamp2",    303, "Mechanical : UVLamp2", 23000, "UVLamp2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLamp3",    304, "Mechanical : UVLamp3", 23000, "UVLamp3", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLamp4",    305, "Mechanical : UVLamp4", 23000, "UVLamp4", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLED1",     306, "Mechanical : UVLED1", 24000, "UVLED1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLED2",     307, "Mechanical : UVLED2", 24000, "UVLED2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLED3",     308, "Mechanical : UVLED3", 24000, "UVLED3", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "UVLED4",     309, "Mechanical : UVLED4", 24000, "UVLED4", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_ML_STAGE     , "Stage1",     310, "Mechanical : Stage1", 17000, "Stage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_STAGE     , "Stage2",     311, "Mechanical : Stage2", 17000, "Stage2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_STAGE     , "Stage3",     312, "Mechanical : Stage3", 17000, "Stage3", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_WORKBENCH , "Workbench",  313, "Mechanical : Work Bench", 21000, "Workbench", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "SHead1",     314, "Mechanical : SHead1", 22000, "SHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "SHead2",     315, "Mechanical : SHead2", 22000, "SHead2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "GHead1",     316, "Mechanical : GHead1", 22000, "GHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_DISPENSER , "GHead2",     317, "Mechanical : GHead2", 22000, "GHead2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_HANDLER   , "UHandler",   318, "Mechanical : UHandler", 19000, "UHandler", LOG_ALL, LOG_DAY ),
		
	            // 350-399 : Control Layer --------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_CL_MANAGE_OP_PANEL   , "ManageOpPanel",     350, "Control : Manage OP Panel", 32200, "C_ManageOpPanel", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_LOADER            , "CtrlLoader",        351, "Control : Loader", 30600, "C_Loader", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_PUSHPULL          , "CtrlPushPull",      352, "Control : PushPull", 30700, "C_PushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_STAGE1            , "CtrlStage1",        353, "Control : Stage1", 30500, "C_Stage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_UNLOAD_HANDLER    , "CtrlUHandler",      356, "Control : UHandler", 30800, "C_UHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_VISION_CALIBRATION, "VisionCalibration", 360, "Control : Vision Calibration1", 31000, "C_VisionCalib1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_VISION_CALIBRATION, "VisionCalibration", 361, "Control : Vision Calibration2", 31000, "C_VisionCalib2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_HW_TEACH          , "HWTeach",           362, "Control : HW Teach", 32000, "C_HWTeach", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_INTERFACE_CTRL    , "InterfaceCtrl1",    363, "Control : Interface Ctrl1", 32400, "C_InterfaceCtrl1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_MANAGE_PRODUCT    , "ManageProduct",     364, "Control : Manage Product", 32300, "C_ManageProduct", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_INTERFACE_CTRL    , "InterfaceCtrl2",    365, "Control : Interface Ctrl2", 32400, "C_InterfaceCtrl2", LOG_ALL, LOG_DAY ),
		
	            // 400-459 : Process Layer --------------------------------------------------------------------
                new CObjectInfo( (int)OBJ_PL_TRS_AUTO_MANAGER  , "TrsAutoManager",   400, "Process : TrsAuto Manager", 40000, "TrsAutoManager", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_LOADER        , "TrsLoader",        401, "Process : TrsLoader", 41000, "TrsLoader", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_PUSHPULL      , "TrsPushPull",      402, "Process : TrsPushPull", 42000, "TrsPushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_STAGE1        , "TrsStage1",        403, "Process : TrsStage1", 43000, "TrsStage1", LOG_ALL, LOG_DAY ),
                //new CObjectInfo( (int)OBJ_PL_TRS_STAGE2        , "TrsStage2",        403, "Process : TrsStage2", 43000, "TrsStage2", LOG_ALL, LOG_DAY ),
                //new CObjectInfo( (int)OBJ_PL_TRS_STAGE3        , "TrsStage3",        404, "Process : TrsStage3", 43000, "TrsStage3", LOG_ALL, LOG_DAY ),
                //new CObjectInfo( (int)OBJ_PL_TRS_WORKBENCH     , "TrsWorkbench",     405, "Process : TrsWorkbench", 45000, "TrsWorkbench", LOG_ALL, LOG_DAY ),
                //new CObjectInfo( (int)OBJ_PL_TRS_DISPENSER     , "TrsDispenser",     406, "Process : TrsDispenser", 44000, "TrsDispenser", LOG_ALL, LOG_DAY ),
                //new CObjectInfo( (int)OBJ_PL_TRS_UNLOAD_HANDLER, "TrsUnloadHandler", 407, "Process : TrsUnloadHandler", 47000, "TrsUnloadHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_JOG           , "TrsJog",           409, "Process : TrsJog", 48000, "TrsJog", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_PL_TRS_LCNET         , "TrsLCNet",         410, "Process : TrsLCNet", 49000, "TrsLCNet", LOG_ALL, LOG_DAY ),
            };

        }

        public bool GetObjectInfo(int ID, out CObjectInfo objInfo)
        {
            objInfo = new CObjectInfo();
            foreach(CObjectInfo objectInfo in arrayObjectInfo)
            {
                if(objectInfo.ID == ID)
                {
                    objInfo = objectInfo;
                    return true;
                }
            }

            //return ERR_SYSTEMINFO_NOT_REGISTED_OBJECTID;
            return false;
        }

        public CObjectInfo GetObjectInfo(int ID)
        {
            CObjectInfo objInfo = new CObjectInfo();
            foreach (CObjectInfo objectInfo in arrayObjectInfo)
            {
                if (objectInfo.ID == ID)
                {
                    objInfo = objectInfo;
                    break;
                }
            }
            return objInfo;
        }
    }
}
