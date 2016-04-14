using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_Cylinder;

namespace LWDicer.Control
{
    public class DEF_Cylinder
    {
        public const int DEF_MAX_CYLINDER_SOLENOID = 4;
        public const int DEF_MAX_CYLINDER_SENSOR = 4;

        public const int ERR_CYLINDER_LOG_NULL_POINTER = 1;
        public const int ERR_CYLINDER_NULL_DATA = 2;
        public const int ERR_CYLINDER_INVALID_POINTER = 3;
        public const int ERR_CYLINDER_TIMEOUT = 4;
        public const int ERR_CYLINDER_INVALID_POS = 5;

        public enum ECylinderTime
        {
            MOVING_TIME, NOSEN_MOVING_TIME, SETTLING_TIME1, SETTLING_TIME2
        };

        public enum ECylinderType
        {
            UP_DOWN,
            LEFT_RIGHT,
            FRONT_BACK,
            UPSTREAM_DOWNSTREAM,
            CW_CCW,
            OPEN_CLOSE,
            UP_MID_DOWN,
            LEFT_MID_RIGHT,
            FRONT_MID_BACK,
            UPSTREAM_MID_DOWNSTREAM,
            UPSTREAM_DOWNSTREAM_VARIOUS_VELOCITY,
            UPSTREAM_MID_DOWNSTREAM_VARIOUS_VELOCITY,
        };

        public enum ESolenoidType
        {
            SINGLE_SOLENOID,
            REVERSE_SINGLE_SOLENOID,
            DOUBLE_SOLENOID,
            DOUBLE_3WAY_SOLENOID,
            DOUBLE_SOLENOID_VARIOUS_VELOCITY
        };

        // Cylinder Component가 가지는 Mechanical Component List
        public class CCylinderRefComp
        {
        }

        public class CCylinderTime
        {
            // @param MovingTime : Cylinder 이동시 걸리는 최대 시간
            public double MovingTime;

            // @param NoSenMovingTIme : Cylinder 이동시 Sensor가 없을때의 Moving Time	
            public double NoSenMovingTime;

            // SettlingTime : Cylinder가 1동작후 안정화 되는데 걸리는 시간
            public double SettlingTime1;

            // SettlingTime : Cylinder가 2동작후 안정화 되는데 걸리는 시간
            public double SettlingTime2;
        }

        public class CCylinderData
        {
            // @link aggregation Cylinder 타입
            public ECylinderType CylinderType;

            // @link aggregation Solenoid 타입
            public ESolenoidType SolenoidType;

            // 생성된 Cylinder 객체와 연관된 Solenoid 단동식일때는 하나 사용, 복동식일때는 2개 사용 
            public int[] Solenoid = new int[2];

            // 생성된 Cylinder 객체와 연관된 가감속 Solenoid  +,- 방향 1개씩  
            public int[] AccSolenoid = new int[2];

            // Up Sensor  : 체크하고자 하는 갯수 만큼 지정 하고 나머지는 NULL로 한다.
            public int[] UpSensor = new int[DEF_MAX_CYLINDER_SENSOR];

            // Down Sensor : 체크하고자 하는 갯수 만큼 지정 하고 나머지는 NULL로 한다.
            public int[] DownSensor = new int[DEF_MAX_CYLINDER_SENSOR];

            // Middle Sensor : 등록된 Sensor들의 상태 체크 
            public int[] MiddleSensor = new int[DEF_MAX_CYLINDER_SENSOR];

            // 가감속 센서 : +방향, - 방향 2개 밖에 지정할 수 없다.
            public int[] AccSensor = new int[2];

            public CCylinderTime Time = new CCylinderTime();

            public CCylinderData()
            {
            }
        }
    }

    public interface ICylinder
    {
        int SetCylinderTime(CCylinderTime time);

        int GetCylinderTime(out CCylinderTime time);

        /*----------- 실린더 상태 확인  -----------------------*/
        /**
        * Cylinder가 Up 되어 있는는지 확인한다.
        * 하나로 여러개의 Solenoid를 움직이는 경우 여러개의 Sensor를 모두 체크 한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsUp(out bool pbStatus);

        /**
        * Cylinder가 Down 되어 있는지 확인한다.
        * 하나로 여러개의 Solenoid를 움직이는 경우 여러개의 Sensor를 모두 체크 한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsDown(out bool pbStatus);

        /**
        * Cylinder가 Left 인지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsLeft(out bool pbStatus);

        /**
        * Cylinder가 Right 인지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsRight(out bool pbStatus);

        /**
        * Cylinder가 Front에 있는지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsFront(out bool pbStatus);

        /**
        * Cylinder가 Front에 있는지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsBack(out bool pbStatus);

        /**
        * Cylinder가 Downstream에 있는지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsDownstr(out bool pbStatus);

        /**
        * Cylinder가 Upstream에 있는지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsUpstr(out bool pbStatus);

        /**
        * Cylinder가 CW에 있는지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsCW(out bool pbStatus);

        /**
        * Cylinder가 CW에 있는지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsCCW(out bool pbStatus);

        /**
        * Cylinder가 Open 인지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsOpen(out bool pbStatus);

        /**
        * Cylinder가 Close 인지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsClose(out bool pbStatus);

        /**
        * Cylinder가 Middle에 있는지 확인한다.
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsMiddle(out bool pbStatus);

        /*----------- 실린더 이동   -----------------------*/
        /**
        * Cylinder를 Up으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Up(bool bSkipSensor = false);

        /**
        * Cylinder를 Down으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Down(bool bSkipSensor = false);

        /**
        * Cylinder를 Left로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Left(bool bSkipSensor = false);

        /**
        * Cylinder를 Right로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Right(bool bSkipSensor = false);

        /**
        * Cylinder를 Front로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Front(bool bSkipSensor = false);

        /**
        * Cylinder를 Back으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Back(bool bSkipSensor = false);

        /**
        * Cylinder를 Downstr으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Downstr(bool bSkipSensor = false);

        /**
        * Cylinder를 Upstr으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Upstr(bool bSkipSensor = false);

        /**
        * Cylinder를 CW로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int CW(bool bSkipSensor = false);

        /**
        * Cylinder를 CCW으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int CCW(bool bSkipSensor = false);

        /**
        * Cylinder를 Open으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Open(bool bSkipSensor = false);

        /**
        * Cylinder를 Close으로 이동시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Close(bool bSkipSensor = false);

        /**
        * Cylinder를 중간 위치에서 정지 시킨다.
        * @param skip_sensor: Unit의 동작을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수
        * @return 0 = Success, 그 외 = Error
        */
        int Middle(bool bSkipSensor = false);//중간정지..3way sv만허용...

        /*----------- 실린더 이동 시작  -----------------------*/
        /**
        * Cylinder를 Up으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartUp();

        /**
        * Cylinder를 Down으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartDown();

        /**
        * Cylinder를 Left로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartLeft();

        /**
        * Cylinder를 Right로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartRight();


        /**
        * Cylinder를 Front로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartFront();

        /**
        * Cylinder를 Back으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartBack();

        /**
        * Cylinder를 Downstr으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartDownstr();

        /**
        * Cylinder를 Upstr으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartUpstr();

        /**
        * Cylinder를 CW로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartCW();

        /**
        * Cylinder를 CCW으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartCCW();

        /**
        * Cylinder를 Open으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartOpen();

        /**
        * Cylinder를 Close으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartClose();

        /**
        * Cylinder를 중간 위치에서 정지 시킨다.
        * @return 0 = Success, 그 외 = Error
        */
        int StartMiddle();//중간정지..3way sv만허용...


        /*----------- 실린더 동작완료때까지 Sleep   -----------------------*/

        /**
        * Cylinder가 Up 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4UpComplete();

        /**
        * Cylinder가 Down 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4DownComplete();

        /**
        * Cylinder가 Left 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4LeftComplete();

        /**
        * Cylinder가 Right 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4RightComplete();

        /**
        * Cylinder가 Front 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4FrontComplete();

        /**
        * Cylinder가 Back 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4BackComplete();

        /**
        * Cylinder가 CW 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4CWComplete();

        /**
        * Cylinder가 CCW 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4CCWComplete();

        /**
        * Cylinder가 Open 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4OpenComplete();

        /**
        * Cylinder가 Close 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4CloseComplete();

        /**
        * Cylinder가 Downstr 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4DownstrComplete();

        /**
        * Cylinder가 Upstr 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4UpstrComplete();

        /**
        * Cylinder가 MiddlePoint 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4MiddleComplete(bool bDir);//중간정지..3way sv만허용...

        /**
        * 모든 Solenoid를 Off한다.
*/
        void OffSolenoid();

        /**
        * Solenoid을 Off한다.
        *
        * @param   bDir : TRUE -> 1동작, FALSE -> 2동작
        */
        void OffSolenoid(bool bDir);

        /**
        * 실린더 동작 완료를 확인한다.
        *
        * @param   bDir  : TRUE -> 1동작, FALSE -> 2동작
        * @param pbStatus : TRUE = OK,  FALSE = NOT OK
        * @return : 0 = Success, 그 외 = IO Device Error Code
        */
        int IsMoveComplete(bool bDir, out bool pbStatus);

        //----------- Component 공통  -----------------------
        int AssignComponents(IIO pIO);
        int SetData(CCylinderData source);
        int GetData(out CCylinderData target);

    }
}
