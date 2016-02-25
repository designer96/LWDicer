using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWDicer.Control
{
    public interface IIO
    {
        /**
 * Hilscher Board와의 Communication을 위한 Driver를 Open하며, Board를 초기화하고 통신 대기 상태가 되게 한다.
 * @precondition 이 함수는 객체가 생성된후 맨처음 한번만 실행한다. 전에 실행했을 경우는 실행하지 말아야한다.
 * @postcondition Hilscher Board가 통신을 위한 준비 상태가 된다.
 * @return 0 = Success, 그외 = Error Number
 */
        int Initialize();

    /**
     * I/O Device의 Digital Status (Bit) 를  읽어드린다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
	 * @param pbVal    : IO 값
     * @return  0      : SUCCESS
	            else   : Device \Error 코드 
     */
    int GetBit(int usIOAddr, out bool pbval) ;

    /**
     * Hilscher Board와의 Communication을 종료하고 Device Driver를 Close한다.
     * @precondition 이 함수를 실행하기 전에 initialize  함수가 미리 실행되었어야 한다.
     * @postcondition Hilscher Board와 통신 종결
      * @return 0 = Success, 그외 = Error Number
     */
    int Terminate() ;

    /**
     * I/O Device의 Digital Status (Bit) 를 읽어들여 bit 값을 아규먼트로 리턴한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
	 * @param pbVal    : TRUE : 값이 1 임, FALSE : 값이 0 임
     * @return  0      : SUCCESS
	            else   : Device Error 코드 
     */
    int IsOn(int usIOAddr, out bool pbVal);

   /**
     * I/O Device의 Digital Status (Bit) 를 읽어들여 bit 값을 아규먼트로 리턴한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
	 * @param pbVal    : TRUE : 값이 0 임, FALSE : 값이 1 임
     * @return  0      : SUCCESS
	            else   : Device Error 코드 
     */
    int IsOff(int usIOAddr, out bool pbVal);

    /**
     * Output Device에 On Command (Bit = 1) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
     * @return 0 = Success, 그외 = Error Number
     */
    int OutputOn(int usIOAddr);

    /**
     * Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
     * @return 0 = Success, 그외 = Error Number
     */
    int OutputOff(int usIOAddr);

    /**
     * Output Device의 Digital Status가 Set이면 (Bit = 0), Output Device에 On Command (Bit = 1) 를 보내고,
     * Output Device의 Digital Status가 Clear이면 (Bit = 1), Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : IO Address
     * @return 0 = Success, 그외 = Error Number
     */
    int OutputToggle(int usIOAddr);

    /**
     * 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address
     * @param pcValuse : 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    int GetByte(int usIOAddr, ref byte pcValue);

    /**
     * 연속된 8개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address
     * @param pcValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    int PutByte(int usIOAddr, byte pcValue);

    /**
     * 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address
     * @param pwValuse : 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    int GetWord(int usIOAddr, ref ushort pwValue);

    /**
     * 연속된 16개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param usIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address
     * @param pwValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    int PutWord(int usIOAddr, ushort pwValue);

    /**
     * I/O Device의 Digital Status (Bit) 를  읽어드린다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
	 * @param pbVal    : IO 값
     * @return  0      : SUCCESS
	            else   : Device \Error 코드 
     */
    int GetBit(string strIOAddr, ref bool pbVal);

    /**
     * I/O Device의 Digital Status (Bit) 를 읽어들여 Bit = 1이면, TRUE(1)를 Return하고, Bit = 0이면 FALSE(0)를 Return한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
	 * @param pbVal    : IO 값
     * @return  0      : SUCCESS
	            else   : Device \Error 코드 
     */
    int IsOn(string strIOAddr, ref bool pbVal);

    /**
     * Output Device에 On Command (Bit = 1) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, 그외 = Error Number
     */
    int OutputOn(string strIOAddr);

    /**
     * Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, 그외 = Error Number
     */
    int OutputOff(string strIOAddr);

    /**
     * Output Device의 Digital Status가 Set이면 (Bit = 0), Output Device에 On Command (Bit = 1) 를 보내고,
     * Output Device의 Digital Status가 Clear이면 (Bit = 1), Output Device에 Off Command (Bit = 0) 를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, 그외 = Error Number
     */
    int OutputToggle(string strIOAddr);

    /**
     * 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pcValuse : 연속된 8개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    int GetByte(string strIOAddr, ref byte pcValue);

    /**
     * 연속된 8개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 8개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pcValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    int PutByte(string strIOAddr, byte pcValue);

    /**
     * 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue pointer에 넘겨준다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pwValuse : 연속된 16개의 IO Address로 구성된 Input Device 들의 Digital Status를 읽어들여 pcValue에 저장한다.
     * @return 0 = Success, 그외 = Error Number
     */
    int GetWord(string strIOAddr, ref ushort pwValue);

    /**
     * 연속된 16개의 IO Address로 구성된 Output Device들에 On or Off Command를 보낸다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @param strIOAddr : 연속된 16개의 IO Address를 시작하는 IO Address의 String Type (ex, "1000:START_SW")
     * @param pwValuse : Output Device에 보낼 Command를 저장하고 있는 변수이다.
     * @return 0 = Success, 그외 = Error Number
     */
    int PutWord(string strIOAddr, ushort pwValue);

    /**
     * Incoming Buffer를 Update하고, Outgoing Buffer의 내용을 Physical I/O에 적용하는 IOThread를 Run한다.
     * @precondition 이 함수를 실행하기 전에 initialize 함수가 미리 실행되었어야 한다.
     * @postcondition Incoming Buffer를 Update하고, Outgoing Buffer의 내용을 Physical I/O에 적용하는 IOThread가 Run한다.
     */
    void RunIOThread();

	/**
	 * Master 모듈 및 Slave 모듈 상태 정보를 얻어온다.
	 *
	 * @param  DnStatus : 마스터와 64개의 Slave에 대한 상태 정보 구조체
	 * @return 0		= 모두 정상
			   others	= 하나라도 실패
	 */
	//int DnStatusGet(DN_STATUS DnStatus);

    }
}
