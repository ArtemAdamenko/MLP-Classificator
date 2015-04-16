function [TRAIN1, TRAIN2] = NeuralNet1()
clear;
[A] = csvread('3.csv'); %чтение csv-файла
[m,n] = size(A); %размерность входного массива

[TRAIN1] = A(1:m,1:(n-2)); % входная серия, то есть первый столбец входного файла
[TRAIN2] = A(1:m,(n-1)); %выходная серия, в данном случае второй столбец
mx = max(TRAIN1);
mx2 = max(TRAIN2);
for i=1:m
    for j=1:27
        TRAIN1(i,j) = TRAIN1(i,j)/mx(j);
        %TRAIN2(i,1) = TRAIN2(i,1)/mx2(1);
    end
end
%TRAIN1 = TRAIN1(1:m)/mx(1); % приводим входной ряд в единичный интервал
%TRAIN2 = TRAIN2(1:m,(n-1))/mx; % приводим выходной ряд в единичный интервал
TRAIN1 = TRAIN1';
TRAIN2 = TRAIN2';

%for k=2:m-1 % в этом цикле делаем нормальную входную и выходную выборки для прогнозирования
% RYD_IN(k-1) = TRAIN1(k+1) - TRAIN1(k); % то есть делаем разность между К-м и К+1 - м значениями ряда
% RYD_OUT(k-1) = TRAIN2(k+1) - TRAIN2(k);
% NUM(k-1) = k-1;
%end

%net = newff(TRAIN1, TRAIN2, [10],{'trainlm'});

%net = newff(TRAIN1, TRAIN2); % создаем нейронную сеть
net=newff(minmax(TRAIN1),[27,20,1],{'logsig' 'logsig' 'purelin'},'trainscg');
net.trainParam.epochs = 1000; % количество эпох для обучения
net.performFcn='sse';
net.trainParam.goal=0.01;

net = train(net, TRAIN1, TRAIN2); % собственно обучение нейросети

y1 = sim(net, TRAIN1); % имитация работы сети

c = y1 - TRAIN2; % разность между тем, что должно было быть и тем, что получилось

plot(c); % вывод графика того, что должно было быть и того что получилось

% если ниже расскоментировать, а предыдущую строчку закоментировать, то будет вывод в отдельных рядах

%subplot(2,1,1); plot(RYD_OUT)
%title('Real data')
%subplot(2,1,2); plot(y1)
%title('NN out')

% или можно выводить ошибку, если расскоментировать
%plot(c);
%-------------------------------------------- 