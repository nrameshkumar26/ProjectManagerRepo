import { Component, NgModule, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CommonServiceService } from './services/common-service.service';
import { Http, Response } from '@angular/http';
import { PagerService } from './services/pageService';
import { AlertsModule } from 'angular-alert-module';
import { OrderPipe, OrderModule } from 'ngx-order-pipe';

declare var $: any;


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [PagerService]
})

@NgModule({
  declarations: [],

  imports: [OrderPipe],
  providers: [],
  bootstrap: [AppComponent]
})

export class AppComponent implements OnInit {
  constructor(private appServices: CommonServiceService, private pageService: PagerService,
    private fb: FormBuilder, private orderPipe: OrderPipe) { }
  title = 'ProjectManager';
  parentTaskList: any;
  taskDetails: any = [];
  userDetails: any = [];
  projDetails: any = [];
  managerDetails: any = [];
  pager: any = {};
  pagedItems: any = [];
  page: number;
  response: any;
  search: any = {
    projSearch: '',
    taskSearch: '',
    userSearch: ''
  }

  projectkey: string = '';
  userkey: string = '';
  taskkey: string = '';
  submitted: boolean = false;
  addUserSubmitted: boolean = false;
  projSubmitted: boolean = false;
  userShow: boolean = false;
  taskShow: boolean = false;
  projectShow: boolean = true;
  myForm: FormGroup;
  addUserForm: FormGroup;
  myProjectForm: FormGroup;
  orderBy: boolean = false;
  isUserUpdate: boolean = false;
  filter = false;
  parentFilter = false;
  StartDate = new Date();

  accepted: any;
  Priority: any;

  public ngAfterContentInit() {

  }


  public ngOnInit() {

    this.appServices.getParentTask().subscribe(data => {
      this.parentTaskList = data;
    });

    this.appServices.getProjectDetails().subscribe(data => {
      this.projDetails = data;
    });

    this.appServices.getManagerDetails().subscribe(data => {
      this.managerDetails = data;
    });

    this.appServices.getUserDetails().subscribe(data => {
      this.userDetails = data;
    });

    this.appServices.getTaskManager().subscribe(data => {
      this.taskDetails = data;
      this.setPage(1);
    });

    this.appServices.getTaskManager();
    this.appServices.getUserDetails();
    this.appServices.getProjectDetails();
    this.appServices.getManagerDetails();

    this.myProjectForm = this.fb.group({
      ProjectId: 0,
      Project: ['', Validators.required],
      Priority: [15, Validators.required],
      StartDate: '',
      EndDate: '',
      ManagerId: ''
    });

    this.myForm = this.fb.group({
      TaskId: 0,
      ProjectId: [0],
      Project: [''],
      Task: ['', Validators.required],
      ParentTask: '',
      Priority: '',
      ParentId: 0,
      StartDate: '',
      EndDate: '',
      IsActive: 0,
      IsParent: false,
      ManagerId: '',
      UserId: ''
    });

    this.addUserForm = this.fb.group({
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      EmployeeId: ['', Validators.required],
      UserId: 0,
      ProjectId: '',
      TaskId: '',
      IsActive: ''
    });
  };

  getProjectDetails() {
    this.appServices.getProjectDetails().subscribe(data => {
      this.projDetails = data;
    });
  };
  getManagerDetails() {
    this.appServices.getManagerDetails().subscribe(data => {
      this.managerDetails = data;
    });
  };

  getTaskManager() {
    this.appServices.getTaskManager().subscribe(data => {
      this.taskDetails = data;
      this.setPage(1);
    });
  };

  getUserDetails() {
    this.appServices.getUserDetails().subscribe(data => {
      this.userDetails = data;
    });
  };

  onProjectSubmit() {
    this.projSubmitted = true;
    if (this.myProjectForm.valid) {
      if (this.compareTwoDates(this.myProjectForm.value)) {
        var pId = this.myProjectForm.value.ProjectId;
        this.appServices.submitProject(this.myProjectForm.value).subscribe(data => {
          if (data) {
            alert(`Project ${pId == 0 ? 'Created' : 'Updated'} successfully...`);
            this.myProjectForm.reset();
            this.submitted = false;
            $('#save-info').prop('checked', false);
            this.onFilterChange();
            this.getProjectDetails();
          }
          else {
            alert('Oops! Something went wrong. Please try again...');
          }
        });
      }
      else {
        alert('End Date should be greater than Start Date');
      }
    }
  };

  onSubmit() {
    this.submitted = true;
    if (this.myForm.valid) {

      if (this.compareTwoDates(this.myForm.value)) {
        var tId = this.myForm.value.TaskId;
        this.appServices.submitTask(this.myForm.value).subscribe(data => {
          if (data) {
            alert(`Task ${tId == 0 ? 'Created' : 'Updated'} successfully...`);
            this.myForm.reset();
            this.submitted = false;
            this.onParentCheck();
            this.getTaskManager();
            this.getProjectDetails();
          }
          else {
            alert('Oops! Something went wrong. Please try again...');
          }
        });
      }
      else {
        alert('End Date should be greater than Start Date');
      }
    }
  };

  public EditTask(task) {
    $('.task-manager-page a[href="#addTask"]').tab('show');
    if (task.StartDate != null)
      task.StartDate = task.StartDate.slice(0, 10);
    if (task.EndDate != null)
      task.EndDate = task.EndDate.slice(0, 10);
    var modelConverter = {
      Project: task.Project,
      ProjectId: task.ProjectId,
      IsParent: task.IsParent,
      Task: task.Task,
      TaskId: task.TaskId,
      StartDate: task.StartDate,
      EndDate: task.EndDate,
      Priority: task.Priority,
      ManagerId: task.ManagerId,
      ParentId: task.ParentId,
      ParentTask: task.ParentTask,
      IsActive: task.IsActive,
      UserId: task.UserId
    };
    this.myForm.setValue(modelConverter);
    if (task.StartDate != null)
      this.parentFilter = false;
  };

  public EndTask(task) {
    this.appServices.updateEndTask(task).subscribe(data => {

      this.getTaskManager();
      alert(`Data updated successfully...`);
      this.getProjectDetails();
    });
  }

  public ResetTask() {
    this.myForm.reset();
    this.submitted = false;
  }

  public ResetProject() {
    this.myProjectForm.reset();
    //this.submitted = false;
  }


  AddUserSubmit() {
    this.addUserSubmitted = true;
    if (this.addUserForm.valid) {
      var uId = this.addUserForm.value.UserId;
      this.appServices.submitUser(this.addUserForm.value).subscribe(data => {
        if (data) {
          alert(`User ${uId == 0 ? 'Added' : 'Updated'} successfully...`);
          this.addUserForm.reset();
          this.submitted = false;
          this.getUserDetails();
        }
        else {
          alert('Oops! Something went wrong. Please try again...');
        }
      });
    }
  };

  public EditUser(user) {
    this.addUserForm.setValue(user);
    this.isUserUpdate = true;
    window.scrollTo(0, 0);
  };

  public DeleteUser(user) {

    user.Action = 'delete';
    this.appServices.submitUser(user).subscribe(data => {
      if (data) {
        alert(`Data Deleted successfully...`);

        this.getUserDetails();
      }
      else {
        alert('Oops! Something went wrong. Please try again...');
      }
    });
  };

  public EditProject(project) {

    if (project.StartDate != null)
      project.StartDate = project.StartDate.slice(0, 10);

    if (project.EndDate != null)
      project.EndDate = project.EndDate.slice(0, 10);
    var modelConverter = {
      StartDate: project.StartDate,
      EndDate: project.EndDate,
      Project: project.Project,
      Priority: project.Priority,
      ProjectId: project.ProjectId,
      ManagerId: project.ManagerId
    };
    this.myProjectForm.setValue(modelConverter);
    if (project.StartDate != null) {
      $('#save-info').prop('checked', true);
      this.onFilterChange();
    }
    window.scrollTo(0, 0);
  };

  public SuspendProject(project) {

    project.Action = 'delete';
    this.appServices.suspendProject(project).subscribe(data => {
      if (data) {
        alert(`Project Suspended successfully...`);

        this.getProjectDetails();
      }
      else {
        alert('Oops! Something went wrong. Please try again...');
      }
    });
  };


  public AddUserResetTask() {
    this.addUserForm.reset();
    this.addUserSubmitted = false;
    this.isUserUpdate = false;
  }


  // Common function

  setPage(page: number) {
    if (this.pager.totalPages != 0) {
      if (page < 1 || page > this.pager.totalPages) {
        return;
      }
    }
    // get pager object from service
    this.pager = this.pageService.getPager(this.taskDetails.length, page);
    // get current page of items
    this.pagedItems = this.taskDetails.slice(this.pager.startIndex, this.pager.endIndex + 1);
  };


  compareTwoDates(data) {
    if (data.EndDate != null && data.EndDate != '') {
      if (new Date(data.EndDate) < new Date(data.StartDate))
        return false;

      else
        return true;
    }
    else {
      return true;
    }
  };

  AddProject() {
    this.projectShow = true;
    this.userShow = false;
    this.taskShow = false;
  };

  AddTask() {
    this.projectShow = false;
    this.userShow = false;
    this.taskShow = false;
  };

  AddUser() {
    this.projectShow = false;
    this.userShow = true;
    this.taskShow = false;
  };

  ViewTask() {
    this.projectShow = false;
    this.userShow = false;
    this.taskShow = true;
  };

  ManagerSearch() {
    $("#myModal").modal();

  };

  onFilterChange() {
    this.filter = !this.filter;
  };

  sortProject(projectkey) {
    this.projDetails = this.orderPipe.transform(this.projDetails, projectkey);
  };

  sortUser(userkey) {
    this.userDetails = this.orderPipe.transform(this.userDetails, userkey);
  };

  sortTask(taskkey) {
    this.pagedItems = this.orderPipe.transform(this.pagedItems, taskkey);
  };


  onParentCheck() {
    this.parentFilter = !this.parentFilter;
  };
  GetManager(manager) {
    this.myProjectForm.controls['ManagerId'].setValue(manager.EmployeeId);
    $("#myModal").modal('hide');
  };

  ProjectSearch() {
    $("#projectModal").modal();
  };

  GetProject(project) {
    this.myForm.controls['Project'].setValue(project.Project);
    this.myForm.controls['ProjectId'].setValue(project.ProjectId);
    $("#projectModal").modal('hide');
  };

  ParentSearch() {
    $("#parentTaskModal").modal();
  };

  GetParentTask(parent) {
    this.myForm.controls['ParentTask'].setValue(parent.ParentTask);
    this.myForm.controls['ParentId'].setValue(parent.ParentId);
    $("#parentTaskModal").modal('hide');
  };

  UserSearch() {
    $("#userModal").modal();
  };

  GetUser(user) {
    this.myForm.controls['ManagerId'].setValue(user.EmployeeId);
    this.myForm.controls['UserId'].setValue(user.UserId);
    $('#ManagerId').val(user.EmployeeId);
    $("#userModal").modal('hide');
  };
}