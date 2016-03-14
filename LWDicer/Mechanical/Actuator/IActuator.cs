using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Actuator;

namespace LWDicer.Control
{
    public class DEF_Actuator
    {
        public const int DEF_DEFAULT_ITEM_COUNT = 5;       // 실린더와 Vacuum의 Default 수용 갯수

        // Error Code 정의
        public const int ERR_ACTUATOR_LOG_NULL_POINTER = 1;
        public const int ERR_ACTUATOR_NULL_DATA = 2;
        public const int ERR_ACTUATOR_INVALIDE_POINTER = 3;
        public const int ERR_ACTUATOR_MEM_ALLOC_FAIL = 4;
        public const int ERR_ACTUATOR_ITEM_FULL = 5;
        public const int ERR_ACTUATOR_INDEX_ERROR = 6;
        public const int ERR_ACTUATOR_TIMEOUT = 7;
        public const int ERR_ACTUATOR_ALL_TIMEOUT = 8;
        public const int ERR_ACTUATOR_UP_FAIL = 9;
        public const int ERR_ACTUATOR_START_FAIL = 10;

        public const int ERR_NOT_IMPLEMETNED_YET = 20;

        // Actuator의 동작 Step을 정의함 
        public enum EActuatorStep
        {
            ACTUATOR_IDLE,
            ACTUATOR_START,
            ACTUATOR_DONE,
            ACTUATOR_START_FAIL
        };


        // Actuator Type을 정의 
        public enum EActuatorType
        {
            ACTUATOR_VACUUM,
            ACTUATOR_CYLIDNER
        };

        public struct SActuatorRefComp
        {
        }

        public struct SActuatorData
        {

        }
    }

    /// <summary>
    /// 실린더와 Vacuum을 그룹으로 묶어서 관리하고자 하는 Component이다.
    /// 각각의 실린더와 Vacuum을 생성하고 파라미터를 초기화 한후 본 Component에 등록 하여야 한다.
    /// 각각의 조작을 Cylinder나 Vacuum을 Index를 지정하고, 복수개를 지정할 경우 Index의 배열을 지정한다.
    /// </summary>
    public interface IActuator
    {
        /**
        * Component가 수용할 실린더 갯수를 지정한다.
        * 이 Interface를 사용해서 지정하지 않으면 Default 갯수를 사용한다.
        * @param iCount : 수용할 실린더의 갯수
        * @return 0 = Success, 그 외 = Error
        */
        int SetCylinderCount(int iCount);

        /**
        * Component가 수용할 Vacuum 갯수를 지정한다.
        * 이 Interface를 사용해서 지정하지 않으면 Default 갯수를 사용한다.
        * @param iCount : 수용할 Vacuum의 갯수
        * @return 0 = Success, 그 외 = Error
        */
        int SetVacuumCount(int iCount);

        /*----------- 실린더 및 Vacuum 등록 및 조회 ------------------------------*/

        /**
        * Component에 Cylinder를 등록 한다.
        * 한번에 하나씩 등록 하여야 하며 수용 갯수를 초과하면 에러(-1)을 리턴한다.
        * @param pCylinder : 등록할 실린더 포인터
        * @return 0 = Success, 그 외 = Error
        */
        int RegisterCylinder(ICylinder pCylinder);
        /**
        * Component에 Vacuum을 등록 한다.
        * 한번에 하나씩 등록 하여야 하며 수용 갯수를 초과하면 에러(-1)을 리턴한다.
        * @param pVacuum : 등록할 실린더 포인터
        * @return 0 = Success, 그 외 = Error
        */
        int RegisterVacuum(IVacuum pVacuum);

        /**
        * Component에 등록된 실린더의 ObjectID를 읽어온다.
        * Index에 해당하는 실린더가 등록되어 있지 않으면 에러(-1)를 리턴한다.
        * @param iIndex : 실린더 인덱스 ( 0 ~ )
        * @return 실린더의 Object ID, 에러시 -1 
        */
        int GetCylinderObjectID(int iIndex);

        /**
        * Component에 등록된 Vacuum의 ObjectID를 읽어온다.
        * Index에 해당하는 Vacuum이 등록되어 있지 않으면 에러(-1)를 리턴한다.
        * @param iIndex : Vacuum 인덱스 ( 0 ~ )
        * @return 실린더의 Object ID, 에러시 -1 
        */
        int GetVacuumObjectID(int iIndex);


        /*----------- 개별 실린더 동작 (이동 완료후 리턴) ------------------------------*/
        // --- UP / Down
        /**
        * 실린더 Up동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error	      
        */
        int Up(int iIndex = -1);
        /**
        * 실린더 Down동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error	      
        */
        int Down(int iIndex = -1);

        // --- Left / Rigth
        /**
        * 실린더 Left 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int Left(int iIndex = -1);
        /**
        * 실린더 Right 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int Right(int iIndex = -1);

        // --- Front / Back
        /**
        * 실린더 Front 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int Front(int iIndex = -1);
        /**
        * 실린더 Back 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
         * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int Back(int iIndex = -1);

        // --- Close / Open
        /**
        * 실린더를 이용하여 Gripper 또는 Clamp를 Close 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int Close(int iIndex = -1);
        /**
        * 실린더를 이용하여 Gripper 또는 Clamp를 Open 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int Open(int iIndex = -1);

        // --- CW / CCW
        /**
        * 실린더를 이용하여 시계방향으로 회전한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int CW(int iIndex = -1);
        /**
        * 실린더를 이용하여 반시계방향으로 회전한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int CCW(int iIndex = -1);

        // --- 중간 위치로 이동 : 3-WAY 실린더에만 해당됨.
        /**
        * 중간 위치로 실린더를 이동 시킨다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int Middle(int iIndex = -1);

        /*----------- 복수 선별 실린더 동작 (이동 완료후 리턴) ------------------------------*/

        // --- UP / Down
        /**
        * 여러 실린더 Up동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수, -1일 경우 등록된 모든 실린더를 동작 시킨다.
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
                   
        */
        int Up(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Down동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수, -1일 경우 등록된 모든 실린더를 동작 시킨다.
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Down(int[] rgiIndexList, int iCount);

        // --- Left / Rigth
        /**
        * 여러 실린더 Left 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Left(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Right 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Right(int[] rgiIndexList, int iCount);

        // --- Front / Back
        /**
        * 여러 실린더 Front 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Front(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Back 이동 동작을 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Back(int[] rgiIndexList, int iCount);

        // --- Close / Open
        /**
        * 실린더를 이용한 여러개의 Gripper 또는 Clamp를 Close 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Close(int[] rgiIndexList, int iCount);
        /**
        * 실린더를 이용하여 여러개의 Gripper 또는 Clamp를 Open 한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Open(int[] rgiIndexList, int iCount);

        // --- CW / CCW
        /**
        * 여러개의 실린더를 시계방향으로 회전한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int CW(int[] rgiIndexList, int iCount);
        /**
        * 여려개의 실린더를 반시계방향으로 회전한다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int CCW(int[] rgiIndexList, int iCount);

        // --- 중간 위치로 이동 : 3-WAY 실린더에만 해당됨.
        /**
        * 중간 위치로 여러개의 실린더를 이동 시킨다.
        * 동작 완료 상태를 센서로 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Middle(int[] rgiIndexList, int iCount);


        /*----------- 개별 실린더 기동 (기동 시키고 바로 리턴) ------------------------------*/
        // --- UP / Down
        /**
        * 실린더 Up동작을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error	      
        */
        int StartUp(int iIndex = -1);
        /**
        * 실린더 Down동작을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error	      
        */
        int StartDown(int iIndex = -1);

        // --- Left / Rigth
        /**
        * 실린더 Left 이동 동작을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartLeft(int iIndex = -1);
        /**
        * 실린더 Right 이동 동작을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartRight(int iIndex = -1);

        // --- Front / Back
        /**
        * 실린더 Front 이동 동작을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartFront(int iIndex = -1);
        /**
        * 실린더 Back 이동 동작을 시작한다.
         * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartBack(int iIndex = -1);

        // --- Close / Open
        /**
        * 실린더를 이용하여 Gripper 또는 Clamp의 Close을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartClose(int iIndex = -1);
        /**
        * 실린더를 이용하여 Gripper 또는 Clamp의 Open을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartOpen(int iIndex = -1);

        // --- CW / CCW
        /**
        * 실린더를 이용하여 시계방향으로 회전을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartCW(int iIndex = -1);
        /**
        * 실린더를 이용하여 반시계방향으로 회전을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartCCW(int iIndex = -1);

        // --- 중간 위치로 이동 : 3-WAY 실린더에만 해당됨.
        /**
        * 중간 위치로 실린더를 이동을 시작한다.
        * @param iIndex : 대상 실린더의 Index, -1일경우 등록된 모든 실린더를 동작 시킨다. 
        * @return 0 = Success, 그 외 = Error
        */
        int StartMiddle(int iIndex = -1);

        /*----------- 복수 선별 실린더 동작 (이동 완료후 리턴) ------------------------------*/

        // --- UP / Down
        /**
        * 여러 실린더 Up동작을 한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패      
        */
        int StartUp(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Down동작을 한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartDown(int[] rgiIndexList, int iCount);

        // --- Left / Rigth
        /**
        * 여러 실린더 Left 이동 동작을 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartLeft(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Right 이동 동작을 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartRight(int[] rgiIndexList, int iCount);

        // --- Front / Back
        /**
        * 여러 실린더 Front 이동 동작을 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartFront(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Back 이동 동작을 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartBack(int[] rgiIndexList, int iCount);

        // --- Close / Open
        /**
        * 실린더를 이용한 여러개의 Gripper 또는 Clamp의 Close를 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartClose(int[] rgiIndexList, int iCount);
        /**
        * 실린더를 이용한 여러개의 Gripper 또는 Clamp의 Open를 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartOpen(int[] rgiIndexList, int iCount);

        // --- CW / CCW
        /**
        * 여래개의 실린더의 시계방향 회전을 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartCW(int[] rgiIndexList, int iCount);
        /**
        * 여러개의 실린더의 반시계방향 회전을 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartCCW(int[] rgiIndexList, int iCount);

        // --- 중간 위치로 이동 : 3-WAY 실린더에만 해당됨.
        /**
        * 중간 위치로 여러개의 실린더의 이동을 시작한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartMiddle(int[] rgiIndexList, int iCount);


        /*----------- 개별 실린더 동작완료 체크 ------------------------------*/

        // Wait For Up/Down Complete
        /**
        * 실린더 Up동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4UpComplete(int iIndex = -1);
        /**
        * 실린더 Down동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4DownComplete(int iIndex = -1);

        // Wait For Left/Right Complete
        /**
        * 실린더 Left동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4LeftComplete(int iIndex = -1);
        /**
        * 실린더 Right동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4RightComplete(int iIndex = -1);

        // Wait For Front/Back Complete
        /**
        * 실린더 Front동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4FrontComplete(int iIndex = -1);
        /**
        * 실린더 Back 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4BackComplete(int iIndex = -1);

        // Wait For Close/Open Complete
        /**
        * 실린더 Close 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4CloseComplete(int iIndex = -1);
        /**
        * 실린더 Open 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4OpenComplete(int iIndex = -1);

        // Wait For CW/CCW Complete
        /**
        * 실린더 CW동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4CWComplete(int iIndex = -1);
        /**
        * 실린더 CCW동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4CCWComplete(int iIndex = -1);

        // Wait For Middle Complete
        /**
        * 실린더 Middle Postion 이동 동작  완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param iIndex : 체크할 실린더의 Index, -1일경우 등록된 모든 실린더 대상으로 한다.
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4MiddleComplete(int iIndex = -1);

        /*----------- 선별 실린더 동작완료 체크 ------------------------------*/

        // Wait For Up/Down Complete
        /**
        * 여러 실린더 Up동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4UpComplete(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Down동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4DownComplete(int[] rgiIndexList, int iCount);

        // Wait For Left/Right Complete
        /**
        * 여러 실린더 Left동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4LeftComplete(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Right동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4RightComplete(int[] rgiIndexList, int iCount);

        // Wait For Front/Back Complete
        /**
        * 여러 실린더 Front동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4FrontComplete(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Back동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4BackComplete(int[] rgiIndexList, int iCount);

        // Wait For Close/Open Complete
        /**
        * 여러 실린더 Close동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4CloseComplete(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 Open동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4OpenComplete(int[] rgiIndexList, int iCount);

        // Wait For CW/CCW Complete
        /**
        * 여러 실린더 CW동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4CWComplete(int[] rgiIndexList, int iCount);
        /**
        * 여러 실린더 CCW동작 완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4CCWComplete(int[] rgiIndexList, int iCount);

        // Wait For Middle Complete
        /**
        * 여러 실린더 Middle Postion 이동  완료될 때까지 기다린다.
        * 지정 TimeOut값 경과시 Timeout을 리턴한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 체크할 실린더 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4MiddleComplete(int[] rgiIndexList, int iCount);


        /*----------- 개별 실린더 상태 확인 ------------------------------*/

        // Up/Down 상태 확인
        /**
        * 실린더가 Up상태인지 확인한다.
        * @param *pbVal : TRUE -> Up, FALSE -> Up상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsUp(out bool pbVal, int iIndex = -1);
        /**
        * 실린더가 Down상태인지 확인한다.
        * @param *pbVal : TRUE -> Down, FALSE -> Down 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsDown(out bool pbVal, int iIndex = -1);

        // Left/Right 상태 확인
        /**
        * 실린더가 Left상태인지 확인한다.
        * @param *pbVal : TRUE -> LEFT, FALSE -> Left 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsLeft(out bool pbVal, int iIndex = -1);
        /**
        * 실린더가 Right상태인지 확인한다.
        * @param *pbVal : TRUE -> RIGHT, FALSE -> RIGHT 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsRight(out bool pbVal, int iIndex = -1);

        // Front/Back 상태 확인
        /**
        * 실린더가 Front상태인지 확인한다.
        * @param *pbVal : TRUE -> Front, FALSE -> Front 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsFront(out bool pbVal, int iIndex = -1);
        /**
        * 실린더가 Back상태인지 확인한다.
        * @param *pbVal : TRUE -> Back, FALSE -> Back 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsBack(out bool pbVal, int iIndex = -1);

        // Close/Open 상태 확인
        /**
        * 실린더가 Close상태인지 확인한다.
        * @param *pbVal : TRUE -> Close, FALSE -> Close 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsClose(out bool pbVal, int iIndex = -1);
        /**
        * 실린더가 Open상태인지 확인한다.
        * @param *pbVal : TRUE -> Open, FALSE -> Open 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsOpen(out bool pbVal, int iIndex = -1);

        // CW/CCW 상태 확인
        /**
        * 실린더가 Left상태인지 확인한다.
        * @param *pbVal : TRUE -> CW, FALSE -> CW 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsCW(out bool pbVal, int iIndex = -1);
        /**
        * 실린더가 Right상태인지 확인한다.
        * @param *pbVal : TRUE -> CCW, FALSE -> CCW 상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsCCW(out bool pbVal, int iIndex = -1);

        // Middle 상태 확인
        /**
        * 실린더가 Middle상태인지 확인한다.
        * @param *pbVal : TRUE -> Up, FALSE -> Up상태가 아님
        * @param iIndex : 확인할 실린더의 Index, -1일경우 등록된 모든 실린더에 대해 확인한다.
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsMiddle(out bool pbVal, int iIndex = -1);

        /*----------- 선별 실린더 동작완료 체크 ------------------------------*/

        // Up/Down 상태 확인
        /**
        * 여러 실린더 Up 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Up, FALSE -> Up상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsUp(int[] rgiIndexList, int iCount, out bool[] pbVal);
        /**
        * 여러 실린더 Down 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Down, FALSE -> Down상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsDown(int[] rgiIndexList, int iCount, out bool[] pbVal);

        // Left/Right 상태 확인
        /**
        * 여러 실린더 Left 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Left, FALSE -> Left상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsLeft(int[] rgiIndexList, int iCount, out bool[] pbVal);
        /**
        * 여러 실린더 Right 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Right, FALSE -> Right상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsRight(int[] rgiIndexList, int iCount, out bool[] pbVal);

        // Front/Back 상태 확인
        /**
        * 여러 실린더 Front 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Front, FALSE -> Front 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsFront(int[] rgiIndexList, int iCount, out bool[] pbVal);
        /**
        * 여러 실린더 Back 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Back, FALSE -> BackWard상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsBack(int[] rgiIndexList, int iCount, out bool[] pbVal);

        // Close/Open 상태 확인
        /**
        * 여러 실린더 Close 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Close, FALSE -> Close 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsClose(int[] rgiIndexList, int iCount, out bool[] pbVal);
        /**
        * 여러 실린더 Open 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Open, FALSE -> Open 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsOpen(int[] rgiIndexList, int iCount, out bool[] pbVal);

        // CW/CCW 상태 확인
        /**
        * 여러 실린더 CW 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> CW, FALSE -> CW 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsCW(int[] rgiIndexList, int iCount, out bool[] pbVal);
        /**
        * 여러 실린더 CCW 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> CCW, FALSE -> CCW 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsCCW(int[] rgiIndexList, int iCount, out bool[] pbVal);

        // Middle 상태 확인
        /**
        * 여러 실린더 Middle 상태를 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Middle, FALSE -> Middle 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsMiddle(int[] rgiIndexList, int iCount, out bool[] pbVal);

        /*----------- 개별 Vacuum 동작 (동작 완료후 리턴) ------------------------------*/
        /**
        * Vacuum을 On 한다.
        * @param iIndex : On할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum On
        * @return 0 = Success, 그 외 = Error
        */
        int VacuumOn(int iIndex = -1);
        /**
        * Vacuum을 Off 한다.
        * @param iIndex : Off할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum On
        * @return 0 = Success, 그 외 = Error
        */
        int VacuumOff(int iIndex = -1);


        /*----------- 선별 Vacuum 동작 (동작 완료후 리턴) ------------------------------*/
        /**
        * 여러 Vacuum을 On 한다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트              
        * @param iCount : 대상 Vacuum 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int VacuumOn(int[] rgiIndexList, int iCount);
        /**
        * 여러 Vacuum을 Off 한다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 Vacuum 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int VacuumOff(int[] rgiIndexList, int iCount);

        /*----------- 개별 Vacuum 동작 시작 ------------------------------*/
        /**
        * Vacuum On을 시작 한다.
        * @param iIndex : On할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum 동시 On
        * @return 0 = Success, 그 외 = Error
        */
        int StartVacuumOn(int iIndex = -1);
        /**
        * Vacuum Off를 시작 한다.
        * @param iIndex : On할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum 동시 Off
        * @return 0 = Success, 그 외 = Error
        */
        int StartVacuumOff(int iIndex = -1);


        /*----------- 선별 Vacuum 동작 시작 ------------------------------*/
        /**
        * 여러 Vacuum On을 시작 한다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 Vacuum 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartVacuumOn(int[] rgiIndexList, int iCount);
        /**
        * 여러 Vacuum Off를 시작 한다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 Vacuum 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int StartVacuumOff(int[] rgiIndexList, int iCount);

        /*----------- 개별 Vacuum 동작 완료될때까지 Sleep ------------------------------*/
        /**
        * Vacuum On 동작이 완료될때까지 기다린다.
        * @param iIndex : 확인할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum 대상
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4VacuumOnComplete(int iIndex = -1);
        /**
        * Vacuum Off 동작이 완료될때까지 기다린다.
        * @param iIndex : 확인할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum 대상
        * @return 0 = Success, 그 외 = Error
        */
        int Wait4VacuumOffComplete(int iIndex = -1);

        /*----------- 선별 Vacuum 동작 완료 체크 ------------------------------*/
        /**
        * 여러 Vacuum On 동작이 완료될때까지 기다린다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 Vacuum 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4VacuumOnComplete(int[] rgiIndexList, int iCount);
        /**
        * 여러 Vacuum Off 동작이 완료될때까지 기다린다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 Vacuum 갯수
        * @return 0 = Success  : 모두 성공
                  그외 = Error : 하나라도 실패
        */
        int Wait4VacuumOffComplete(int[] rgiIndexList, int iCount);


        /*----------- 개별 Vacuum 상태 확인  ------------------------------*/
        /**
        * Vacuum On 상태인지 확인한다.
        * @param iIndex : 확인할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum 동시 체크
        * @param *pbVal : TRUE -> Vacuum On, FALSE -> Vacuum On 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsVacuumOn(out bool pbVal, int iIndex = -1);
        /**
        * Vacuum이 Off인지 확인한다.
        * @param iIndex : Vacuum의 Index, 0부터 시작
        * @param *pbVal : TRUE -> Vacuum Off, FALSE -> Vacuum Off 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsVacuumOff(out bool pbVal, int iIndex = -1);


        /*----------- 선별 Vacuum 상태 확인  ------------------------------*/
        /**
        * 여러 Vacuum 대상으로  On 상태인지 확인한다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 Vacuum 갯수
        * @param iCount : 대상 Vacuum 갯수
        * @param *pbVal : TRUE -> Vacuum On, FALSE -> Vacuum On 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsVacuumOn(int[] rgiIndexList, int iCount, out bool[] pbVal);
        /**
        * 여러 Vacuum 대상으로 Off 상태인지 확인한다.
        * @param rgiIndexList :
             Input 대상 Vacuum Index 리스트
             Output 결과 리스트
        * @param iCount : 대상 Vacuum 갯수
        * @param *pbVal : TRUE -> Vacuum Off, FALSE -> Vacuum Off 상태가 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int IsVacuumOff(int[] rgiIndexList, int iCount, out bool[] pbVal);


        /*----------- 개별 Vacuum Compare  ------------------------------*/
        /**
        * 출력값과 센서값을 비교하여 같은지 확인한다.
        * @param iIndex : 확인할 Vacuum의 Index, -1일 경우 등록된 모든 Vacuum 동시 체크
        * @param *pbVal : TRUE -> Compare OK, FALSE -> Compare Ok 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int CompareVacuum(out bool pbVal, int iIndex = -1);

        /*----------- 선별 Vacuum Compare  ------------------------------*/
        /**
        * 여러 Vacuum을 대상으로 출력값과 센서값을 비교하여 같은지 확인한다.
        * @param rgiIndexList :
             Input 대상 실린더 Index 리스트
             Output 결과 리스트
        * @param iCount : 확인할 실린더 갯수
        * @param *pbVal : TRUE -> Compare OK, FALSE -> Compare Ok 아님
        * @return  0    : 성공
                   그외 : IO Device 에러 코드
        */
        int CompareVacuum(int[] rgiIndexList, int iCount, out bool[] pbVal);


        //----------- Component 공통  -----------------------
        int SetData(SActuatorData source);
        int GetData(out SActuatorData target);

    }
}
