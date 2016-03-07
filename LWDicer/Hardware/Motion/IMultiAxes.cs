using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_Motion;

namespace LWDicer.Control
{
    public interface IMultiAxes
    {
        /**
         * Motion Component를 초기화한다.
         *
         * 1. Motion Board 종류에 맞는 Motion Library 개체를 생성한다.
         * 2. 축 구성 개수를 설정한다.
         * 3. 축 정보를 설정한다.
         *
         * @param	iObjectID		: Object ID
         * @param	iBoardType		: Motion Board Type (1=MMC Board, 2=MEI board, 3=PPC Board, ...)
         * @param	iAxesNum		: 축 구성 개수
         * @param	*saxAxis		: 1축 구성 정보 (축 구성 수만큼 배열로 존재)
         */
        int Initialize(int iBoardType, int iAxesNum, ref CAxis1[] saxAxis);

        /**
         * 해당 축의 현재까지 쌓인 Frame을 Clear한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         */
        int ClearFrames(int iCoordinateID);

        /**
         * MMC 보드의 비어있는 Frame 갯수를 돌려준다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         */
        int FramesLeft(int iCoordinateID, int[] piFrameNo);

        int SetStop(int iCoordinateID, int iType = DEF_STOP);

        /**
         * 축 구성 개수를 설정한다. (축 정보 설정과는 별개로 동작한다.)
         *
         * @param   iAxesNum        : 축 구성 개수 (0 ~ 64)
         */
        int SetAxesNumber(int iAxesNum);


        /**
         * 축 구성 개수를 읽는다.
         *
         * @param   *piAxesNum      : 축 구성 개수 (0 ~ 64)
         */
        int GetAxesNumber(out int piAxesNum);


        /**
         * 축 이동 우선순위를 설정한다.
         * 축 정보에 우선순위 설정 후 우선순위별 축 ID를 재정렬한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPriority      : 축 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         */
        int SetAxesMovePriority(int iCoordinateID, int[] pdPriority);


        /**
         * 축 이동 우선순위를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *dpPriority     : 축 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         */
        int GetAxesMovePriority(int iCoordinateID, out int[] pdPriority);


        /**
         * 축 원점복귀 이동 우선순위를 설정한다.
         * 축 정보에 우선순위 설정 후 우선순위별 축 ID를 재정렬한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPriority      : 축 원점복귀 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         */
        int SetAxesOriginPriority(int iCoordinateID, int[] pdPriority);


        /**
         * 축 원점복귀 이동 우선순위를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *dpPriority     : 축 원점복귀 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         */
        int GetAxesOriginPriority(int iCoordinateID, out int[] pdPriority);


        /**
         * 축 1개 또는 구성된 축 모두에 대한 Data를 설정한다. (구조체)
         *
         * 1. 전체 축 모두 설정할 경우
         *	(1) 기 정보 영역이 존재하면 제거한다.
         *  (2) 영역을 새로 allocation한다.
         *	(3) 새 영역에 정보를 설정한다. 이때 축이 이미 사용되고 있으면 안된다.
         * 2. 축 하나만 설정할 경우
         *	(1) 축 정보 영역이 있어야 한다.
         *	(2) 축이 이미 사용되고 있으면 안된다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   pax1Data        : 설정할 각 축의 설정 Data, iCoordinateID의 위치에 기록)
         */
        int SetAxisData(int iCoordinateID, CAxis1[] ax1Data);


        /**
         * 축 1개 또는 구성된 축 모두에 대한 Data를 읽는다. (구조체)
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   ax1Data[]       : 설정할 각 축의 설정 Data, iCoordinateID의 위치에 기록)
         */
        int GetAxisData(int iCoordinateID, out CAxis1[] ax1Data);


        /** 
         * Board에 대한 자동 가, 감속 사용여부를 설정한다.
         *
         * @param   iBoardNo        : MMC Board 번호 0 ~ 7, -1 = All Board
         * @param   *pbAutoSet      : 자동 가,감속 설정여부, true : 수동, false : 자동, iBoardNo=-1이면 배열로 구성
         */
        int SetAutoCP(int iBoardNo, bool[] pbAutoSet);


        /** 
          * Board에 대한 자동 가, 감속 사용여부를 읽는다.
          *
          * @param   iBoardNo        : MMC Board 번호 0 ~ 7, -1 = All Board
          * @param   *pbAutoSet      : 자동 가,감속 설정여부, true : 수동, false : 자동, iBoardNo=-1이면 배열로 구성
          */
        int GetAutoCP(int iBoardNo, out bool[] pbAutoSet);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Axis ID를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *piID           : 설정할 iAxisID, iCoordinateID=-1이면 배열로 제공
         */
        int SetAxisID(int iCoordinateID, int[] piID);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Axis ID를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *piID           : 읽은 iAxisID, iCoordinateID=-1이면 배열로 제공
         */
        int GetAxisID(int iCoordinateID, out int[] piID);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Home 위치(원점복귀위치)를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pPosition      : 설정할 dHomePosition, iCoordinateID=-1이면 배열로 제공
         */
        int SetHomePosition(int iCoordinateID, double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Home 위치(원점복귀위치)를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 읽은 dHomePosition, iCoordinateID=-1이면 배열로 제공
         */
        int GetHomePosition(int iCoordinateID, out double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Negative Limit 위치를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 설정할 dNegativeLimit Position, iCoordinateID=-1이면 배열로 제공
         */
        int SetNegativePosition(int iCoordinateID, double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Negative Limit 위치를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 읽은 dNegativeLimit Position, iCoordinateID=-1이면 배열로 제공
         */
        int GetNegativePosition(int iCoordinateID, out double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Positive Limit 위치를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 설정할 dPositiveLimit Position, iCoordinateID=-1이면 배열로 제공
         */
        int SetPositivePosition(int iCoordinateID, double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Positive Limit 위치를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 읽은 dPositiveLimit Position, iCoordinateID=-1이면 배열로 제공
         */
        int GetPositivePosition(int iCoordinateID, out double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Moving속도, 가속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 설정할 dMovingVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 설정할 iMovingAccelerate, iCoordinateID=-1이면 배열로 제공
         */
        int SetMovingVelocity(int iCoordinateID, double[] pdVelocity, int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Moving속도, 가속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 읽은 dMovingVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 읽은 iMovingAccelerate, iCoordinateID=-1이면 배열로 제공
         */
        int GetMovingVelocity(int iCoordinateID, out double[] pdVelocity, out int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Coarse속도, 가속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 설정할 dCoarseVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 설정할 iCoarseAccelerate, iCoordinateID=-1이면 배열로 제공
         */
        int SetCoarseVelocity(int iCoordinateID, double[] pdVelocity, int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Coarse속도, 가속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 읽은 dCoarseVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 읽은 iCoarseAccelerate, iCoordinateID=-1이면 배열로 제공
         */
        int GetCoarseVelocity(int iCoordinateID, out double[] pdVelocity, out int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Fine속도, 가속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 설정할 dFineVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 설정할 iFineAccelerate, iCoordinateID=-1이면 배열로 제공
         */
        int SetFineVelocity(int iCoordinateID, double[] pdVelocity, int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Fine속도, 가속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 읽은 dFineVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 읽은 iFineAccelerate, iCoordinateID=-1이면 배열로 제공
         */
        int GetFineVelocity(int iCoordinateID, out double[] pdVelocity, out int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Jog Move의 Pitch, 속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPitch         : 설정할 dJogPitch, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 설정할 dJogVelocity, iCoordinateID=-1이면 배열로 제공
         */
        int SetJogVelocity(int iCoordinateID, double[] pdPitch, double[] pdVelocity);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Jog Move의 Pitch, 속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPitch         : 읽은 dJogPitch, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 읽은 dJogVelocity, iCoordinateID=-1이면 배열로 제공
         */
        int GetJogVelocity(int iCoordinateID, out double[] pdPitch, out double[] pdVelocity);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Sign을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbSign          : 설정할 bSign, iCoordinateID=-1이면 배열로 제공
         */
        int SetSign(int iCoordinateID, bool[] pbSign);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Sign을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbSign          : 읽은 bSign, iCoordinateID=-1이면 배열로 제공
         */
        int GetSign(int iCoordinateID, out bool[] pbSign);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행방향을 설정한다.
         *   Limit Sensor 구성에 따른 원점복귀 초기 진행방향을 설정할 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         */
        int SetOriginDir(int iCoordinateID, bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행방향을 읽는다.
         *   Limit Sensor 구성에 따른 원점복귀 초기 진행방향을 읽을 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         */
        int GetOriginDir(int iCoordinateID, out bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행(Fine구간)방향을 설정한다.
         *   Fine 속도 구간에서 초기 진행방향을 설정할 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         */
        int SetOriginFineDir(int iCoordinateID, bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행(Fine구간)방향을 읽는다.
         *   Fine 속도 구간에서 초기 진행방향을 읽을 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         */
        int GetOriginFineDir(int iCoordinateID, out bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 C상 사용여부를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbUse           : 설정할 bCPhaseUse, iCoordinateID=-1이면 배열로 제공
         */
        int SetCPhaseUse(int iCoordinateID, bool[] pbUse);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 C상 사용여부를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbUse           : 읽은 bCPhaseUse, iCoordinateID=-1이면 배열로 제공
         */
        int GetCPhaseUse(int iCoordinateID, out bool[] pbUse);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Scale을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdScale         : 설정할 dScale, iCoordinateID=-1이면 배열로 제공
         */
        int SetScale(int iCoordinateID, double[] pdScale);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Scale을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdScale         : 읽은 dScale, iCoordinateID=-1이면 배열로 제공
         */
        int GetScale(int iCoordinateID, out double[] pdScale);


        /** 
         * 축 이동 시 지연 시간을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정할 이동 지연 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         */
        int SetMoveTime(int iCoordinateID, double[] pdTime);


        /** 
         * 축 이동 시 지연 시간을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정된 이동 지연 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         */
        int GetMoveTime(int iCoordinateID, out double[] pdTime);


        /** 
         * 축 이동 후 안정화 시간을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정할 이동 후 안정화 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         */
        int SetMoveAfterTime(int iCoordinateID, double[] pdTime);


        /** 
         * 축 이동 후 안정화 시간을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정된 이동 후 안정화 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         */
        int GetMoveAfterTime(int iCoordinateID, out double[] pdTime);


        /** 
         * 축 위치 허용 오차를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTolerance     : 설정할 위치 허용 오차 (mm단위), iCoordinateID=-1이면 배열로 제공
         */
        int SetTolerance(int iCoordinateID, double[] pdTolerance);


        /** 
         * 축 위치 허용 오차를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTolerance     : 설정된 위치 허용 오차 (mm단위), iCoordinateID=-1이면 배열로 제공
         */
        int GetTolerance(int iCoordinateID, out double[] pdTolerance);


        /** 
         * 축 원점복귀 완료 대기 시간(초)을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정할 원점복귀 완료 대기 시간 (초 단위), iCoordinateID=-1이면 배열로 제공
         */
        int SetOriginWaitTime(int iCoordinateID, double[] pdTime);


        /** 
         * 축 원점복귀 완료 대기 시간(초)을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정된 원점복귀 완료 대기 시간 (초 단위), iCoordinateID=-1이면 배열로 제공
         */
        int GetOriginWaitTime(int iCoordinateID, out double[] pdTime);


        /**
         * 축이 원점복귀 됐는지 확인한다. (한개의 축 혹은 구성된 모든 축에 대해 가능)
         * 모든 축에 대한 원점복귀 확인 시 오류 Code는 전달되지 않는다.
         * 확인하고자 하는 축에 대해 오류 Code를 읽어봐야 한다.
         * 
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param	*pbResult        : (OPTION = null) 축에 대한 원점복귀 여부 종합 상태
         * @param   *pbStatus        : (OPTION = null) 각 축마다 원점복귀 여부 읽기, iCoordinateID=-1이면 배열로 제공
         */
        int IsOriginReturn(int iCoordinateID, out bool[] pbResult, out bool[] pbStatus);


        /**
         * 축 원점복귀 해제하기 (한개의 축 혹은 구성된 모든 축에 대해 가능)
         * 
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbReturn        : (OPTION = null) 원점복귀 결과 읽기, iCoordinateID=-1이면 배열로 제공
         */
        int ResetOrigin(int iCoordinateID, bool[] pbReturn = null);


        /**
         * 축 원점복귀 하기 (한개의 축 혹은 구성된 모든 축에 대해 가능)
         * 한번의 명령 수행 후 원점복귀가 완전히 종료된 후 다음 원점복귀를 수행할 수 있다. 만약 명령을 연달아 2번 이상 실행하게 되면 최종 명령만 수행하게 된다.
         * 
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param	*pbUse           : 원점복귀할 축 지정, iCoordinateID=-1일때만 사용
         *                                                 iCoordinateID가 -1이 아니면 사용안함 (null)
         *                                                 배열구조에 사용하고자하는 축 위치에 true지정
         * @param	bMoveOpt         : 원점복귀 시 이동할 지 여부, true=이동 포함
         */
        int ReturnOrigin(int iCoordinateID, bool[] pbUse, bool bMoveOpt);


        /**
         * 축 원점복귀 강제 종료하기 (구성된 모든 축에 대해 동작 정지 명령 수행)
         * 
         */
        int StopReturnOrigin();


        /**
         * 축의 현재좌표를 읽는다.
         * 
         * @param   iCoordinateID        : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdCurrentPosition   : 현재 좌표값, iCoordinateID=-1이면 배열로 제공
         * @param   bType                : 읽을 위치 종류, false=실제위치, true=목표위치
         */
        int GetCurrentPosition(int iCoordinateID, out double[] pdCurrentPosition, bool bType);


        /**
         * 축의 현재좌표를 설정한다.
         * 
         * @param   iCoordinateID        : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdCurrentPosition   : 현재 좌표값, iCoordinateID=-1이면 배열로 제공
         * @param   bType                : 읽을 위치 종류, false=실제위치, true=목표위치
         */
        int SetCurrentPosition(int iCoordinateID, double[] pdCurrentPosition, bool bType);


        /**
         * 축의 현재좌표와 특정좌표간의 수치에 의한 좌표차이를 비교한다.
         * 
         * @param   iCoordinateID        : 구성 축 배열 Index, -1 = All Axis
         * @param	bPosOpt              : 비교할 위치 종류, false=현재위치, true=Motion의 목표위치
         * @param   *pdTargetPosition    : 비교할 좌표값, iCoordinateID=-1이면 배열로 제공
         * @param   *pdPermission        : 비교허용 오차, iCoordinateID=-1이면 배열로 제공, null이면 내부 Tolerance값으로 비교한다.
         * @param   *pbJudge             : 비교결과, iCoordinateID=-1이면 배열로 제공
         * @param   *pdDeviation         : (OPTION = null) 비교 차이값, iCoordinateID=-1이면 배열로 제공
         */
        int ComparePosition(int iCoordinateID, bool bPosOpt, double[] pdTargetPosition, double[] pdPermission,
                                     bool[] pbJudge, double[] pdDeviation = null);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 이동)의 축에 대한 이동) - 이동 완료된 후 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdPosition      : 이동할 위치, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         *										 4=비대칭 사다리꼴 속도 Profile, 절대좌표 이동
         *										 5=비대칭 S-Curve 속도 Profile, 절대좌표 이동
         * @param   bPriority        : 우선순위 이동 사용 여부, true=사용, false=동시이동
         */
        int Move(int iCoordinateID, bool[] pbMoveUse, double[] pdPosition, double[] pdVelocity,
                          int[] piAccelerate, int[] piDecelerate, int iMoveType, bool bPriority);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 무시)의 축에 대한 이동) - 이동 명령 후 바로 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdPosition      : 이동할 위치, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         *										 4=비대칭 사다리꼴 속도 Profile, 절대좌표 이동
         *										 5=비대칭 S-Curve 속도 Profile, 절대좌표 이동
         */
        int StartMove(int iCoordinateID, bool[] pbMoveUse, double[] pdPosition, double[] pdVelocity,
                               int[] piAccelerate, int[] piDecelerate, int iMoveType);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 이동)의 축에 대한 상대위치 이동) - 이동 완료된 후 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdDistance      : 이동할 거리, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 2=사다리꼴 속도 Profile, 상대거리 이동
         *										 3=S-Curve 속도 Profile, 상대거리 이동
         *										 6=비대칭 사다리꼴 속도 Profile, 상대거리 이동
         *										 7=비대칭 S-Curve 속도 Profile, 상대거리 이동
         * @param   bPriority        : 우선순위 이동 사용 여부, true=사용, false=동시이동
         * @param	bClearOpt        : (OPTION=false) 이동 전과 후에 Encoder 값을 Clear하는 동작 사용 여부 (true:사용, false:미사용)
         */
        int RMove(int iCoordinateID, bool[] pbMoveUse, double[] pdDistance, double[] pdVelocity,
                           int[] piAccelerate, int[] piDecelerate, int iMoveType, bool bPriority, bool bClearOpt = false);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 무시)의 축에 대한 상대위치 이동) - 이동 명령 후 바로 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdDistance      : 이동할 거리, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 2=사다리꼴 속도 Profile, 상대거리 이동
         *										 3=S-Curve 속도 Profile, 상대거리 이동
         *										 6=비대칭 사다리꼴 속도 Profile, 상대거리 이동
         *										 7=비대칭 S-Curve 속도 Profile, 상대거리 이동
         * @param	bClearOpt        : (OPTION=false) 이동 전과 후에 Encoder 값을 Clear하는 동작 사용 여부 (true:사용, false:미사용)
         */
        int StartRMove(int iCoordinateID, bool[] pbMoveUse, double[] pdDistance, double[] pdVelocity,
                                int[] piAccelerate, int[] piDecelerate, int iMoveType, bool bClearOpt = false);


        /**
         * 축 이동 (한개의 축에 대한 등속 이동, 등속 위치까진 가속 이동함) 
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 허용안됨
         * @param   dVelocity        : 이동할 속도, 0.0 = 지정된 속도 사용
         * @param   iAccelerate      : 이동할 가속도, 0.0 = 지정된 가속도 사용
         * @param   bDir             : (OPTION=true) 이동할 방향, true:(+), false:(-), 생략하면 (+방향으로 이동
         *                             dVelocity에 값을 넣어주면 bDir은 생략해서 사용하면 된다.
         *                             이 경우는 dVelocity의 부호에 의해 이동 방향이 결정된다.
         *                             dVelocity에 0.0을 넣어 지정된 속도를 사용하는 경우는
         *                             bDir로 (+/-) 방향을 설정할 수 있다.
         *                             만약, dVelocity에 값을 넣은 경우 bDir을 설정을 하게 되면
         *                             지정된 dVelocuty, dAccelerate에 bDir이 반영되어 이동을 하게 된다.
         */
        int VMove(int iCoordinateID, double dVelocity, int iAccelerate, bool bDir = true);


        /**
         * 축 이동 후 완료를 확인한다. (한개의 축 또는 여러 개의 축에 대한 완료 확인) 
         * 
         * @param   iCoordinateID    : 조회할 축 지정, -1 = All Axis
         * @param   *pbUse           : 이동 완료 확인할 축 지정, iCoordinateID=-1일때만 사용
         *                                                 iCoordinateID가 -1이 아니면 사용안함 (null)
         *                                                 배열구조에 사용하고자하는 축 위치에 true지정
         * @param   bSkipMode        : (OPTION=false) 위치 확인 대기, 이동 후 안정화 시간 지연 사용 여부
         */
        int Wait4Done(int iCoordinateID, bool[] pbUse, bool bSkipMode = false);


        /**
         * 축이 이동 완료되었는지 확인한다. (확인 후 바로 return한다.)
         * Motion 동작이 완료되었는지, Motion 동작과 In-Position되었는지 확인할 수 있는 기능을 제공한다.
         * 축 1개만 지정하여 확인할 수 있고, 모든 축을 다 확인할 수 있다.
         *
         * @param	iCoordinateID	: 좌표 Index (-1:All좌표)
         * @param	*pbDone			: 이동 완료 확인 결과 (true=이동 완료완료되었음, false=)이동완료 안되었음
         * @param	bMode			: (OPTION=false) 대기 종류, false=이동 및 속도이동 완료,
         *										 true=이동 및 속도이동 완료 & InPosition 범위내 이동 완료
         */
        int CheckDone(int iCoordinateID, out bool[] pbDone, bool bMode = false);


        /**
         * 직선보간 이동한다.
         * 
         * @param   iMaxAxNo         : 직선보간에 사용할 축 개수
         * @param   *pbUse           : 직선보간에 사용할 축 지정, 구성 축 개수만큼 배열 중 사용할 축에 true 지정
         * @param   iMaxPoint        : 직선보간 이동구간 개수
         * @param   *pdPosition      : 직선보간 이동구간 지정, (iMaxPoint X iMaxAxis)만큼 설정
         * @param   *pdVelocity      : 이동 시 속도, iMaxAxis만큼 설정
         * @param   *piAccelerate    : 이동 시 가속도, iMaxAxis만큼 설정
         */
        int MoveSplineLine(int iMaxAxNo, bool[] pbUse, int iMaxPoint,
                                    double[] pdPosition, double[] pdVelocity, int[] piAccelerate);


        /**
         * 원호보간 이동한다.
         * 
         * @param   iMaxAxNo         : 원호보간에 사용할 축 개수
         * @param   *pbUse           : 원호보간에 사용할 축 지정, 구성 축 개수만큼 배열 중 사용할 축에 true 지정
         * @param   iMaxPoint        : 원호보간 이동구간 개수
         * @param   *pCenter         : 원호보간시 곡률 중심점 지정 (x, y), iMaxPoint만큼 설정
         * @param   *pdPosition      : 원호보간 이동구간 지정, (iMaxPoint X iMaxAxis)만큼 설정
         * @param   *pdVelocity      : 이동 시 속도, iMaxAxis만큼 설정
         * @param   *piAccelerate    : 이동 시 가속도, iMaxAxis만큼 설정
         * @param   *pbDir           : 원호보간 시 회전방향 설정, 1 = CIR_CCW (반시계방향), 0 = CIR_CW (시계방향),
         *											iMaxPoint만큼 설정
         */
        int MoveSplineArc(int iMaxAxNo, bool[] pbUse, int iMaxPoint, double[] pCenter,
                                   double[] pdPosition, double[] pdVelocity, int[] piAccelerate, bool[] pbDir);


        /**
         * Jog Pitch에 의한 이동한다.
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 허용안됨
         * @param   bDir             : 이동할 방향, true:(+), false:(-)
         * @param   dPitch           : (OPTION = 0.0) 이동할 거리, 0.0 = 지정된 Pitch거리 사용
         */
        int JogMovePitch(int iCoordinateID, bool bDir, double dPitch = 0.0);


        /**
         * Jog Velocity에 의한 이동한다.
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 허용안됨
         * @param   bDir             : 이동할 방향, true:(+), false:(-)
         * @param   dVelocity        : (OPTION = 0.0) 이동할 속도, 0.0 = 지정된 속도 사용
         */
        int JogMoveVelocity(int iCoordinateID, bool bDir, double dVelocity = 0.0);


        /**
         * 축을 정지한다. (한개의 축 혹은 모든 축에 대한 정지)
         * 
         * @param   iCoordinateID    : 정지할 축 지정, -1 = All Axis
         * @param   *pbStatus      : (OPTION = null) 각 축의 Stop 상태, iCoordinateID=-1이면 배열로 구성
         */
        int Stop(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축을 등속이동에 대해 정지한다. (한개의 축 혹은 구성된 모든 축의 등속이동에 대한 정지)
         * 
         * @param   iCoordinateID    : 정지할 축 지정, -1 = All Axis
         * @param   *pbState         : (OPTION = null) 각 축의 VStop 상태, iCoordinateID=-1이면 배열로 구성
         */
        int VStop(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축을 비상정지한다. (한개의 축 혹은 구성된 모든 축에 대한 비상정지)
         * 
         * @param   iCoordinateID    : 정지할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 EStop 상태, iCoordinateID=-1이면 배열로 구성
         */
        int EStop(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Servo를 On 한다. (한개의 축 혹은 구성된 모든 축에 대한 Servo On 수행)
         * 
         * @param   iCoordinateID    : Servo ON 할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 Servo ON 상태, iCoordinateID=-1이면 배열로 구성
         */
        int ServoOn(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Servo를 Off 한다. (한개의 축 혹은 구성된 모든 축에 대한 Servo Off 수행)
         * 
         * @param   iCoordinateID    : Servo OFF 할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 Servo OFF 상태, iCoordinateID=-1이면 배열로 구성
         */
        int ServoOff(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Home Sensor 상태를 읽는다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbStatus        : 각 축의 Home Sensor 상태, iCoordinateID=-1이면 배열로 구성
         */
        int CheckHomeSensor(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Positive Sensor 상태를 읽는다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbStatus        : 각 축의 Positive Sensor 상태, iCoordinateID=-1이면 배열로 구성
         */
        int CheckPositiveSensor(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Negative Sensor 상태를 읽는다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbStatus        : 각 축의 Negative Sensor 상태, iCoordinateID=-1이면 배열로 구성
         */
        int CheckNegativeSensor(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Home Sensor에 대한 Event 및 Level을 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         */
        int SetHomeSensorLimit(int iCoordinateID, int[] piLimit, bool[] pbLevel);


        /**
         * 축의 Home Sensor에 대한 Event를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         */
        int SetHomeSensorEvent(int iCoordinateID, int[] piLimit);


        /**
         * 축의 Home Sensor에 대한 Level을 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         */
        int SetHomeSensorLevel(int iCoordinateID, bool[] pbLevel);


        /**
         * 축의 Positive Sensor에 대한 Event 및 Limit를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         */
        int SetPositiveSensorLimit(int iCoordinateID, int[] piLimit, bool[] pbLevel);


        /**
         * 축의 Positive Sensor에 대한 Event를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         */
        int SetPositiveSensorEvent(int iCoordinateID, int[] piLimit);


        /**
         * 축의 Positive Sensor에 대한 Limit를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         */
        int SetPositiveSensorLevel(int iCoordinateID, bool[] pbLevel);


        /**
         * 축의 Negative Sensor에 대한 Event 및 Level를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         */
        int SetNegativeSensorLimit(int iCoordinateID, int[] piLimit, bool[] pbLevel);


        /**
         * 축의 Negative Sensor에 대한 Event 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         */
        int SetNegativeSensorEvent(int iCoordinateID, int[] piLimit);


        /**
         * 축의 Negative Sensor에 대한 Level를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         */
        int SetNegativeSensorLevel(int iCoordinateID, bool[] pbLevel);


        /**
         * 축의 상태(Source)를 읽는다. 
         * 
         * @param   iCoordinateID    : 상태 읽을 축 지정, -1 = All Axis
         * @param   *piSource        : 축 하나에 대한 상태 (Source), iCoordinateID=-1이면 배열로 구성
         */
        int GetAxisStatus(int iCoordinateID, out int[] piSource);


        /**
         * 축의 상태(State)를 읽는다. 
         * 
         * @param   iCoordinateID    : 상태 읽을 축 지정, -1 = All Axis
         * @param   *piSource        : 축 하나에 대한 상태 (State), iCoordinateID=-1이면 배열로 구성
         */
        int GetAxisState(int iCoordinateID, out int[] piState);


        /**
         * 축의 AMP Enable 상태를 읽는다. 
         * 
         * @param   iCoordinateID    : AMP 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbEnable        : 축 하나에 대한 AMP상태, 혹은 모든 축의 AMP 상태 종합
         *							   모든축이 AMP Enable : true, 그외 : false
         * @param   *pbStatus        : (OPTION = null) 각 축의 AMP 상태, iCoordinateID=-1이면 배열로 구성
         */
        int GetAmpEnable(int iCoordinateID, out bool[] pbEnable, out bool[] pbStatus);


        /**
         * 축의 AMP Fault 상태를 읽는다. 
         * 
         * @param   iCoordinateID    : AMP Fault 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbFault         : 축 하나에 대한 AMP Fault상태, 혹은 모든 축의 AMP Fault 상태 종합,
         *							   축 하나라도 Fault 이면 : true, 축 모두 Fault 아니면 : false
         * @param   *pbStatus        : (OPTION = null) 각 축의 AMP Fault상태, iCoordinateID=-1이면 배열로 구성
         */
        int GetAmpFault(int iCoordinateID, out bool[] pbFault, out bool[] pbStatus);


        /**
         * 축의 AMP Fault 상태를 Clear/Enable 한다. 
         * 
         * @param   iCoordinateID    : AMP Fault 상태 설정할 축 지정, -1 = All Axis
         * @param   *pbSet           : 각 축의 설정할 AMP Fault상태, true : Set, false : Reset, iCoordinateID=-1이면 배열로 구성
         * @param   *pbStatus        : (OPTION = null) 각 축의 AMP Fault Enable상태, iCoordinateID=-1이면 배열로 구성
         */
        int SetAmpFault(int iCoordinateID, bool[] pbSet, bool[] pbStatus = null);


        /**
         * 원점 복귀 시 Encoder의 C상 펄스 이용 여부를 읽는다.
         *
         * @param   iCoordinateID	: 초기화할 축 지정, -1 = All Axis
         * @param	*pbIndexReq		: C상 펄스 사용 여부, true =Home Sensor와 Encoder의 Index Pulse를 동시 검출,
         *												  false=Home Sensor만 검출
         *												  iCoordinateID=-1이면 배열로 구성
         */
        int GetIndexRequired(int iCoordinateID, out bool[] pbIndexReq);


        /**
         * 원점 복귀 시 Encoder의 C상 펄스 이용 여부를 설정한다.
         *
         * @param   iCoordinateID	: 초기화할 축 지정, -1 = All Axis
         * @param	*pbIndexReq		: C상 펄스 사용 여부, true =Home Sensor와 Encoder의 Index Pulse를 동시 검출,
         *												  false=Home Sensor만 검출
         *												  iCoordinateID=-1이면 배열로 구성
         */
        int SetIndexRequired(int iCoordinateID, bool[] pbIndexReq);


        /**
         * 축의 상태를 초기화 한다. (한개의 축 혹은 구성된 모든 축에 대해 초기화)
         *  Clear Status & Clear Frames
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 초기화 상태, iCoordinateID=-1이면 배열로 구성
         */
        int ClearAxis(int iCoordinateID, out bool[] pbStatus);


        /**
         * 원점복귀 Step을 설정한다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         * @param   iStep            : 설정값 (0:시작, 999:오류, 1000:완료, 그외:동작중)
         */
        int SetOriginStep(int iCoordinateID, int iStep);


        /**
         * 원점복귀 Step을 읽는다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         * @param   *piStep          : Step값 (0:시작, 999:오류, 1000:완료, 그외:동작중)
         * @param   *piPrevStep      : (OPTION=null) Error전 Step값 (0:시작, 999:오류, 1000:완료, 그외:동작중)
         */
        int GetOriginStep(int iCoordinateID, out int[] piStep, out int[] piPrevStep);


        /**
         * 원점복귀 Error를 초기화한다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         */
        int ClearOriginError(int iCoordinateID);


        /**
         * 원점복귀 Error를 읽는다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         * @param   *piError         : 발생한 오류 Code
         */
        int GetOriginError(int iCoordinateID, out int[] piError);

	    /**
	     * 지정 축이 지정된 위치를 지날 때 지정 IO를 출력한다.
	     *
	     * @param   iCoordinateID    : 축 지정, -1 = 허용안함
	     * @param	siPosNum		: 위치 번호, 1 ~ 10
	     * @param	siIONum			: I/O 번호, 양의정수=ON, 음의정수=OFF
	     * @param	dPosition		: 지정 축의 위치값
	     * @param	bEncFlag		: Encoder Flag, false=내부위치 Counter 사용, true=외부 Encoder 사용
	     */
	    int PositionIoOnoff(int iCoordinateID, int iPosNum, int iIONum, double dPosition, int nEncFlag);

        /**
         * PositionIoOnOff()로 설정된 것을 해제한다.
         *
         * @param   iCoordinateID    : 축 지정, -1 = 허용안함
         * @param	siPosNum		: (OPTION=0) 위치 번호, 1 ~ 10, 0=모든 위치 해제
         */
        int PositionIOClear(int iCoordinateID, int iPosNum = 0);

        int PositionCompare(int iCoordinateID, int iIndexNum, int iBitNo, double dPosition, bool bOutOn);

    }
}
