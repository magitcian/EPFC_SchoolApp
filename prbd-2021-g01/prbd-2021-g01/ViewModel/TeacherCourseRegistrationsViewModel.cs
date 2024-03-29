﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;
using prbd_2021_g01.Model;
using System.Collections;

namespace prbd_2021_g01.ViewModel {
    public class TeacherCourseRegistrationsViewModel : ViewModelCommon {

        private ObservableCollectionFast<Registration> registrations; // = new ObservableCollectionFast<Registration>();
        public ObservableCollectionFast<Registration> Registrations {
            get => registrations;
            set => SetProperty(ref registrations, value);
        }

        private string filter;
        public string Filter {
            get => filter;
            set => SetProperty<string>(ref filter, value, ApplyFilterAction);
        }

        private void ApplyFilterAction() {
            var query = from s in Registration.GetInactiveStudentsByCourse(course) 
                        where s.Firstname.Contains(Filter) || s.Lastname.Contains(Filter) 
                        select s;
            InactiveStudents = new ObservableCollectionFast<Student>(query);
        }

        private ObservableCollectionFast<Student> inactiveStudents;
        public ObservableCollectionFast<Student> InactiveStudents {
            get { return inactiveStudents; }
            set {
                inactiveStudents = value;
                RaisePropertyChanged(nameof(InactiveStudents));
            }
        }

        private ObservableCollectionFast<Registration> activeAndPendingStudents;
        public ObservableCollectionFast<Registration> ActiveAndPendingStudents {
            get { return activeAndPendingStudents; }
            set {
                activeAndPendingStudents = value;
                RaisePropertyChanged(nameof(ActiveAndPendingStudents));
            }
        }

        private Course course;
        public Course Course {
            get => course;
            set => SetProperty(ref course, value, OnRefreshData);
        }

        private IList inactiveStudentSelectedItems = new ArrayList();
        public IList InactiveStudentSelectedItems {
            get => inactiveStudentSelectedItems;
            set => SetProperty(ref inactiveStudentSelectedItems, value);
        }

        private IList activeAndPendingStudentSelectedItems = new ArrayList();
        public IList ActiveAndPendingStudentSelectedItems {
            get => activeAndPendingStudentSelectedItems;
            set => SetProperty(ref activeAndPendingStudentSelectedItems, value);
        }

        public ICommand UnregAllSelect { get; set; }
        public ICommand UnregSelect { get; set; }
        public ICommand RegSelect { get; set; }
        public ICommand RegAllSelect { get; set; }
        public ICommand ActivateSelect { get; set; }
        public ICommand ClearFilter { get; set; }

        public TeacherCourseRegistrationsViewModel() : base() {
            UnregAllSelect = new RelayCommand(MakeThemAllInactiveAction);
            UnregSelect = new RelayCommand(MakeInactiveAction);
            RegSelect = new RelayCommand(MakeActiveAction);
            RegAllSelect = new RelayCommand(MakeThemAllActiveAction);
            ActivateSelect = new RelayCommand(ActivatePendingAction);
            ClearFilter = new RelayCommand(() => Filter = "");
        }

        protected override void OnRefreshData() {
            InactiveStudents = new ObservableCollectionFast<Student>(Registration.GetInactiveStudentsByCourse(course));
            ActiveAndPendingStudents = new ObservableCollectionFast<Registration>(Registration.GetActiveAndPendingRegistrationsByCourse(course));
        }

        public void MakeThemAllInactiveAction() {
            course.makeInactiveStudents(activeAndPendingStudents);
            ResetAndNotify();
        }

        public void MakeInactiveAction() {
            course.makeInactiveStudents(activeAndPendingStudentSelectedItems);
            ResetAndNotify();
        }

        public void MakeActiveAction() {
            course.makeActiveStudents(inactiveStudentSelectedItems);
            ResetAndNotify();
        }

        public void MakeThemAllActiveAction() {
            course.makeActiveStudents(inactiveStudents);
            ResetAndNotify();
        }

        private void ResetAndNotify() { // notify() {
            InactiveStudents.Reset(Registration.GetInactiveStudentsByCourse(course)); // check if refresh in db or cache ?
            ActiveAndPendingStudents.Reset(Registration.GetActiveAndPendingRegistrationsByCourse(course));
            RaisePropertyChanged();
            NotifyColleagues(AppMessages.MSG_STUDENT_CHANGED);
        }

        public void ActivatePendingAction() {
            course.activatePending(activeAndPendingStudentSelectedItems);
            ResetAndNotify();
        }

    }
}
