_G["TestAIPlayer"] = class(AIPlayer, function(c)
	AIPlayer.init(c)	-- must init base!
	c.CurrentTask = nil
end)

function TestAIPlayer:typename()
	return "TestAIPlayer"
end

function TestAIPlayer:initializePlayer()
	debugMsg("Initialisiere TestAIPlayer-KI ...")
	--self.Stats = BusinessStats()
	--self.Stats:Initialize()
	self.Budget = BudgetManager()
	self.Budget:Initialize()
	--self.Requisitions = {}
	--self.NameX = "zzz"
end

function TestAIPlayer:initializeTasks()
	self.TaskList = {}
	self.TaskList["TestTask1"]	= TestTask()
	self.TaskList["TestTask2"]	= TestTask()
end

function TestAIPlayer:GetRequisitionPriority(taskId)
	return 0
end


_G["TestTask"] = class(AITask, function(c)
	AITask.init(c)	-- must init base!
	c.BudgetWeigth = 1
	c.BasePriority = 1
	c.NeededInvestmentBudget = 150000
	c.BaseInvestmentPriority = 5	
end)

function TestTask:typename()
	return "TestTask"
end