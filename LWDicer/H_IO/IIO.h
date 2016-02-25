#ifndef IIO_H
#define IIO_H

#include "DefIO.h"

/**
 * This is an interface class for the IO component.
 * 각 Hischer Board당 하나의 I/O Component가 대응한다 (하나의 CHilscherDnet 객체 생성)
 * @author Jeongseung Moon
 * version 1.0
 * @interface
 */
class IIO 
{
public:  
	/************************************************************************/
	/* 소멸자                                                                     */
	/************************************************************************/
	virtual ~IIO() {};
	
    /**
     * Hilscher Board와의 Communication을 위한 Driver를 Open하며, Board를 초기화하고 통신 대기 상태가 되게 한다.
     * @precondition 이 함수는 객체가 생성된후 맨처음 한번만 실행한다. 전에 실행했을 경우는 실행하지 말아야한다.
     * @postcondition Hilscher Board가 통신을 위한 준비 상태가 된다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int Initialize() = 0;

    /**
     * I/O Device의 Digital Status (Bit) 를  읽어드린다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
	 * @param pbVal    : IO 값
     * @return  0      : SUCCESS
	            else   : Device \Error 코드 
     */
    virtual int GetBit(unsigned short usIOAddr, BOOL *pbval)  = 0;

    /**
     * Hilscher Board와의 Communication을 종료하고 Device Driver를 Close한다.
     * @precondition 이 함수를 실행하기 전에 initialize  함수가 미리 실행되었어야 한다.
     * @postcondition Hilscher Board와 통신 종결
      * @return 0 = Success, 그외 = Error Number
     */
    virtual int Terminate()  = 0;

    /**
     * I/O Device의 Digital Status (Bit) 를 읽어들여 bit 값을 아규먼트로 리턴한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
	 * @param pbVal    : TRUE : 값이 1 임, FALSE : 값이 0 임
     * @return  0      : SUCCESS
	            else   : Device Error 코드 
     */
    virtual int IsOn(unsigned short usIOAddr, BOOL *pbVal) = 0;

   /**
     * I/O Device의 Digital Status (Bit) 를 읽어들여 bit 값을 아규먼트로 리턴한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
	 * @param pbVal    : TRUE : 값이 0 임, FALSE : 값이 1 임
     * @return  0      : SUCCESS
	            else   : Device Error 코드 
     */
    virtual int IsOff(unsigned short usIOAddr, BOOL *pbVal) = 0;

    /**
     * Output Device에 On Command (Bit = 1) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int OutputOn(unsigned short usIOAddr) = 0;

    /**
     * Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int OutputOff(unsigned short usIOAddr) = 0;

    /**
     * Output Device의 Digital Status가 Set이면 (Bit = 0), Output Device에 On Command (Bit = 1) 를 보내고,
     * Output Device의 Digital Status가 Clear이면 (Bit = 1), Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int OutputToggle(unsigned short usIOAddr) = 0;

    /**
     * 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address
     * @param pcValuse : 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int GetByte(unsigned short usIOAddr, BYTE & pcValue) = 0;

    /**
     * 연속된 8개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address
     * @param pcValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int PutByte(unsigned short usIOAddr, BYTE pcValue) = 0;

    /**
     * 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address
     * @param pwValuse : 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int GetWord(unsigned short usIOAddr, WORD & pwValue) = 0;

    /**
     * 연속된 16개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address
     * @param pwValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int PutWord(unsigned short usIOAddr, WORD pwValue) = 0;

    /**
     * I/O Device의 Digital Status (Bit) 를  읽어드린다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
	 * @param pbVal    : IO 값
     * @return  0      : SUCCESS
	            else   : Device \Error 코드 
     */
    virtual int GetBit(CString strIOAddr, BOOL *pbVal) = 0;

    /**
     * I/O Device의 Digital Status (Bit) 를 읽어들여 Bit = 1이면, TRUE(1)를 Return하고, Bit = 0이면 FALSE(0)를 Return한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
	 * @param pbVal    : IO 값
     * @return  0      : SUCCESS
	            else   : Device \Error 코드 
     */
    virtual int IsOn(CString strIOAddr, BOOL *pbVal) = 0;

    /**
     * Output Device에 On Command (Bit = 1) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int OutputOn(CString strIOAddr) = 0;

    /**
     * Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int OutputOff(CString strIOAddr) = 0;

    /**
     * Output Device의 Digital Status가 Set이면 (Bit = 0), Output Device에 On Command (Bit = 1) 를 보내고,
     * Output Device의 Digital Status가 Clear이면 (Bit = 1), Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int OutputToggle(CString strIOAddr) = 0;

    /**
     * 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pcValuse : 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int GetByte(CString strIOAddr, BYTE & pcValue) = 0;

    /**
     * 연속된 8개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pcValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int PutByte(CString strIOAddr, BYTE pcValue) = 0;

    /**
     * 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pwValuse : 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int GetWord(CString strIOAddr, WORD & pwValue) = 0;

    /**
     * 연속된 16개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pwValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    virtual int PutWord(CString strIOAddr, WORD pwValue) = 0;

    /**
     * Incoming Buffer를 Update하고, Outgoing Buffer의 내용을 Physical I/O에 적용하는 IOThread를 Run한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @postcondition Incoming Buffer를 Update하고, Outgoing Buffer의 내용을 Physical I/O에 적용하는 IOThread가 Run한다.
     */
    virtual void RunIOThread() = 0;

	/**
	 * Master 모듈 및 Slave 모듈 상태 정보를 얻어온다.
	 *
	 * @param  DnStatus : 마스터와 64개의 Slave에 대한 상태 정보 구조체
	 * @return 0		= 모두 정상
			   others	= 하나라도 실패
	 */
	virtual int DnStatusGet(DN_STATUS DnStatus) = 0;


/*----------- Component 공통  -----------------------*/

   /**
     * Error Code Base를 설정한다. 
	 *
	 * @param	iErrorBase : (OPTION=0) 설정할 Error Base 값
     */
    virtual void SetErrorBase(int iErrorBase = 0) = 0;

    /**
     * Error Code Base를 읽는다. 
	 *
	 * @return	int : Error Base 값
     */
    virtual int GetErrorBase(void) const = 0;


   /**
     * Object ID를 설정한다. 
	 *
	 * @param	iObjectID : 설정할 Object ID 값
     */
    virtual void SetObjectID(int iObjectID) = 0;

    /**
     * Object ID를 읽는다. 
	 *
	 * @return	int : Object ID 값
     */
    virtual int GetObjectID(void) = 0;

	/** 
	 * Log manager를 반환한다.
	 *
	 * @return	MLog* : 반환할 Log Manager Pointer
	 */
	virtual MLog* GetLogManager() = 0;	

	/** 
	 * Log Class의 개체 Pointer를 설정한다.
	 *
	 * @param		*pLogObj: 연결할 Log Class의 개체 Pointer
	 * @return		Error Code : 0 = Success, 그 외 = Error
	 */
	virtual int SetLogObject(MLog *pLogObj) = 0;

   /**
     * Log File과 관련된 attribute를 지정한다.
     *
	 * @param	iObjectID : ObjectID
     * @param	strFileName : file path 및 file name을 가지는 string
     * @param	ucLevel : log level 지정 bitwise 정보
     * @param	iDays : (OPTION=30) 현재 set되어 있는 log file 저장일
     * @return	Error Code : 0 = Success, 그 외 = Error
     */
    virtual int SetLogAttribute (int iObjectID, CString strFullFileName, BYTE ucLevel, int iDays = 30) = 0;
    /**
     * 오래된 Log file을 삭제한다.
     *
     * @return	Error Code : 0 = Success, 그 외 = Error
     */
    virtual int DeleteOldLogFiles (void) = 0;

	/** 
	 * Component의 Error Code Base를 반환한다.
	 *
	 * @param		Error Code: ObjectID + Error Base 
	 * @return		ErrorBase가 제거된 Component Error Code 
	 */
	virtual int DecodeError(int iErrCode) = 0;

};
#endif //IIO_H
