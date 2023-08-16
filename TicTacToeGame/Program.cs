using System;
using System.Reflection;

namespace TicTacToe
{
    internal class Program
    {
        //틱택토의 맵 상태를 저장할 배열에서, 아직 선택되지 않은 구간을 나타내는 문자.
        static char notAssigned = '?';
        static char[,] arr = new char[3, 3]; // 틱택토를 표현할 배열

        //플레이어의 정보를 저장한 구조체
        struct Player
        {
            public int id;
            public string mark;
        }

        static void Main(string[] args)
        {
            int whosMark = 0;
            int countTurn = 0;
            
            bool isP1Turn = true;
            bool isEnd = false;

            Player player1 = new Player(); player1.id = 1;
            Player player2 = new Player(); player2.id = 2;

            //배열 초기화
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++)
                {
                    arr[i, j] = notAssigned;
                }
            }

            //플레이어가 사용할 표식 선택
            while (true)
            {
                Console.Write("플레이어 1, 당신이 사용할 표식을 고르세요. 플레이어 2는 남은 표식을 자동으로 선택하게 됩니다.");
                Console.Write("(1 입력 - X , 2 입력 - O) : ");
                whosMark = Convert.ToInt32(Console.ReadLine());

                if (whosMark == 1)
                {
                    player1.mark = "X";
                    player2.mark = "O";
                    Console.Clear();
                    break;
                }
                else if (whosMark == 2)
                {
                    player1.mark = "O";
                    player2.mark = "X";
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("숫자 1 또는 2를 입력해주세요.\n");
                }
            }

            // 9 턴 동안 게임 진행
            do
            {
                countTurn++;
                SetGame(player1, player2, isP1Turn);//이번 수의 게임 진행

                //이번 수에서 게임이 끝났다면
                if (CheckIsEnd()) { Console.WriteLine("isEnd!"); isEnd = true; break; }

                isP1Turn = !isP1Turn;//플레이어 턴 전환
            } while(countTurn < 9);


            //모든 칸을 채울 때 까지 승부가 나지 않았다면 무승부.
            if (countTurn >= 9 && !isEnd)
            {
                Console.WriteLine("무승부!");
            }
            else if(countTurn <= 9 && isEnd)
            {
                //해당 플레이어의 턴에서 승부가 났다면 해당 플레이어가 승리한 것.
                if (isP1Turn) { Console.WriteLine("플레이어 1, 승리!"); }
                else { Console.WriteLine("플레이어 2, 승리!"); }
            }
        }

        //한 턴을 진행하는 핵심 함수
        static void SetGame(Player player1, Player player2, bool isP1Turn)
        {
            bool isError = false;
            int index = 0;

            while (true)
            {
                Console.Clear();

                if (isError) { Console.WriteLine("직전에 잘못된 입력이 감지되었습니다. 남은 자리들 중에서 입력하세요. \n"); isError = !isError; }

                Console.WriteLine("플레이어 1 : " + player1.mark + " , " + "플레이어 2 : " + player2.mark + "\n");

                if (isP1Turn)
                {
                    Console.WriteLine("플레이어 1 의 차례 \n");
                }
                else
                {
                    Console.WriteLine("플레이어 2 의 차례 \n");
                }

                //틱택토 맵 보여주기
                SetMap();

                Console.Write("이번 수를 놓을 위치를 입력하세요 : ");

                index = Convert.ToInt32(Console.ReadLine());

                //입력받는 숫자 범위에서 벗어났다면
                if (index < 1 || index > 9)
                {
                    isError = true;
                }
                //이미 수가 놓인 위치를 입력했다면
                else if (CheckIsExist(index))
                {
                    isError = true;
                }
                //수 놓기 입력이 올바르다면 
                else
                {
                    //입력대로 틱택토 배열의 내용을 바꾸고 while 문 종료
                    if (isP1Turn)
                    {
                        Console.WriteLine("isP1Turn! index = " + index);
                        SetMark(index, player1.mark[0]);
                        break;
                    }
                    else
                    {
                        SetMark(index, player2.mark[0]);
                        break;
                    }

                }
            }
        }

        //현재 맵 상태 보여주기
        static void SetMap()
        {
            int count = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    count++;

                    if(arr[i, j] == notAssigned)
                    {
                        Console.Write(count + "  ");
                    }
                    else
                    {
                        Console.Write(arr[i,j] + "  ");
                    }
                }
                Console.WriteLine("\n");
            }
        }

        //해당 위치에 배정된 수가 있는지 확인하는 함수
        static bool CheckIsExist(int index)
        {
            int count = 0;
            bool isExist = false;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    count++;

                    //입력받은 위치에 이미 수가 배정되어 있다면 true 반환, 아니라면 false 반환
                    if (count == index)
                    {
                        if (arr[i, j] != notAssigned) { isExist = true; break; }
                        else { isExist = false; break; }
                    }
                }
            }

            return isExist;
        }

        //이번 턴의 수로 승부가 났는지의 여부를 bool 변수로 반환하는 함수
        static bool CheckIsEnd()
        {
            //가로로 한 줄이 된다면 끝
            if (arr[0, 0] == arr[0, 1] && arr[0, 1] == arr[0, 2] && arr[0,0] != notAssigned) { return true; }
            else if (arr[1, 0] == arr[1, 1] && arr[1, 1] == arr[1, 2] && arr[1, 0] != notAssigned) { return true; }
            else if (arr[2, 0] == arr[2, 1] && arr[2, 1] == arr[2, 2] && arr[2, 0] != notAssigned) { return true; }
            //세로로 한 줄이 나온다면 끝
            if (arr[0, 0] == arr[1, 0] && arr[1, 0] == arr[2, 0] && arr[0, 0] != notAssigned) { return true; }
            else if (arr[0, 1] == arr[1, 1] && arr[1, 1] == arr[2, 1] && arr[0, 1] != notAssigned) { return true; }
            else if (arr[0, 2] == arr[1, 2] && arr[1, 2] == arr[2, 2] && arr[0, 2] != notAssigned) { return true; }
            //대각선으로 한 줄이 나온다면 끝
            if (arr[0, 0] == arr[1, 1] && arr[1, 1] == arr[2, 2] && arr[0, 0] != notAssigned) { return true; }
            else if (arr[0, 2] == arr[1, 1] && arr[1, 1] == arr[2, 0] && arr[0, 2] != notAssigned) { return true; }
            //이도 저도 아니라면 게임은 계속된다.

            return false;
        }

        //입력받은 수를 틱택토 배열에 반영하는 함수
        static void SetMark(int index, char mark)
        {
            int count = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    count++;

                    //입력받은 위치에 해당 마크 배정
                    if (count == index)
                    {                      
                        arr[i, j] = mark;
                        Console.WriteLine(arr[i,j]);
                        break;
                    }
                }
            }
        }
    }
}